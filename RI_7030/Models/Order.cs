using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RI_7030.Models
{
    public enum OrderStatus
    {
        Pending,
        InProduction,
        ReadyToDispatch,
        Delivered,
        Cancelled
    }

    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        // ── Customer ────────────────────────────────────
        [Required(ErrorMessage = "Customer name is required.")]
        [StringLength(150)]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(200)]
        [Display(Name = "Customer Email")]
        public string? CustomerEmail { get; set; }

        [Phone]
        [Display(Name = "Customer Phone")]
        public string? CustomerPhone { get; set; }

        // ── Product ──────────────────────────────────────
        [Required(ErrorMessage = "Product is required.")]
        [StringLength(150)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [Range(0, 10_000_000)]
        [Display(Name = "Unit Price (₹)")]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        [NotMapped]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount => Quantity * UnitPrice;

        // ── Dates ────────────────────────────────────────
        [Display(Name = "Order Date")]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        // ── Status ───────────────────────────────────────
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [StringLength(500)]
        public string? Notes { get; set; }

        // ── Production Stage Completion ──────────────────
        // Each stage matches an EmployeeType
        [Display(Name = "Casting Done")]
        public bool IsCastingDone { get; set; } = false;

        [Display(Name = "Finishing Touch Done")]
        public bool IsFinishingTouchDone { get; set; } = false;

        [Display(Name = "Gold Plating Done")]
        public bool IsGoldPlatingDone { get; set; } = false;

        [Display(Name = "Packaging Done")]
        public bool IsPackagingDone { get; set; } = false;

        // ── Computed ─────────────────────────────────────
        [NotMapped]
        public int CompletedStages =>
            (IsCastingDone        ? 1 : 0) +
            (IsFinishingTouchDone ? 1 : 0) +
            (IsGoldPlatingDone    ? 1 : 0) +
            (IsPackagingDone      ? 1 : 0);

        [NotMapped]
        public int CompletionPercentage => CompletedStages * 25;

        [NotMapped]
        public string CurrentStageLabel => CompletedStages switch
        {
            0 => "Stage 1 – Casting",
            1 => "Stage 2 – Finishing Touch",
            2 => "Stage 3 – Gold Plating",
            3 => "Stage 4 – Packaging",
            _ => "Complete ✔"
        };

        [NotMapped]
        public bool IsOverdue => DueDate.Date < DateTime.Today && Status != OrderStatus.Delivered && Status != OrderStatus.Cancelled;
    }
}
