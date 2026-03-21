using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RI_7030.Models
{
    /// <summary>
    /// Product in inventory — string PK: RI_1001, RI_1002, ...
    /// </summary>
    public class Product
    {
        [Key]
        [StringLength(20)]
        public string ProductId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(150)]
        [Display(Name = "Product Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        [Display(Name = "Stock Quantity")]
        public int Stock { get; set; }

        /// <summary>True when Stock &lt; 15</summary>
        public bool LowStock { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Unit Cost (₹)")]
        public decimal UnitCost { get; set; }

        // ── Production Stage Flags ───────────────────────────
        [Display(Name = "Casting Done")]
        public bool Stage_Casting { get; set; }

        [Display(Name = "Finishing Touch Done")]
        public bool Stage_Finishing { get; set; }

        [Display(Name = "Gold Plating Done")]
        public bool Stage_GoldPlating { get; set; }

        [Display(Name = "Packaging Done")]
        public bool Stage_Packaging { get; set; }

        // ── Computed (not mapped) ────────────────────────────
        [NotMapped]
        public int CompletedStages =>
            (Stage_Casting     ? 1 : 0) +
            (Stage_Finishing   ? 1 : 0) +
            (Stage_GoldPlating ? 1 : 0) +
            (Stage_Packaging   ? 1 : 0);

        [NotMapped]
        public int CompletionPercentage => CompletedStages * 25;

        [NotMapped]
        public bool IsComplete => CompletedStages == 4;

        [NotMapped]
        [Display(Name = "Current Stage")]
        public string CurrentStageLabel => CompletedStages switch
        {
            0 => "Stage 1 – Casting",
            1 => "Stage 2 – Finishing Touch",
            2 => "Stage 3 – Gold Plating",
            3 => "Stage 4 – Packaging",
            _ => "Complete ✔"
        };
    }
}
