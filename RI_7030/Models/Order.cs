using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RI_7030.Models
{
    /// <summary>
    /// Customer order — RI_2001, RI_2002, ...
    /// Status: Pending | In Production | Ready | Delivered
    /// </summary>
    public class Order
    {
        [Key]
        [StringLength(20)]
        public string OrderId { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string CustomerName { get; set; } = string.Empty;

        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? ProductId { get; set; }

        [StringLength(150)]
        public string? ProductName { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public DateTime DueDate { get; set; }

        /// <summary>Pending | In Production | Ready | Delivered</summary>
        [StringLength(30)]
        public string Status { get; set; } = "Pending";

        // ── Production stage flags (C/F/G/P) ───────────────────────
        public bool Stage_C { get; set; }
        public bool Stage_F { get; set; }
        public bool Stage_G { get; set; }
        public bool Stage_P { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // ── Computed ────────────────────────────────────────────────
        [NotMapped]
        public int CompletedStages =>
            (Stage_C ? 1 : 0) +
            (Stage_F ? 1 : 0) +
            (Stage_G ? 1 : 0) +
            (Stage_P ? 1 : 0);

        [NotMapped]
        public int Progress => CompletedStages * 25;

        [NotMapped]
        public bool IsOverdue =>
            DueDate.Date < DateTime.Today && Status != "Delivered";
    }
}
