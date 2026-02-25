using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RI_7030.Models
{
    /// <summary>
    /// Represents a product in inventory.
    /// Every product passes through 4 employee stages before it is complete:
    ///   Stage 1 – Casting
    ///   Stage 2 – Finishing Touch
    ///   Stage 3 – Gold Plating
    ///   Stage 4 – Packaging
    /// </summary>
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(150)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;

        // Stores the relative or absolute path / URL to the uploaded image
        [Display(Name = "Product Image")]
        public string? ImagePath { get; set; }

        [Required(ErrorMessage = "Stock quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        // ── Production Stage Completion Flags ───────────────────────────────
        // Each stage is handled by a specific EmployeeType.
        // Mark true once that stage's employee has completed their work.

        [Display(Name = "Casting Done")]
        public bool IsCastingDone { get; set; } = false;

        [Display(Name = "Finishing Touch Done")]
        public bool IsFinishingTouchDone { get; set; } = false;

        [Display(Name = "Gold Plating Done")]
        public bool IsGoldPlatingDone { get; set; } = false;

        [Display(Name = "Packaging Done")]
        public bool IsPackagingDone { get; set; } = false;

        // ── Computed Properties ─────────────────────────────────────────────

        /// <summary>
        /// Returns how many of the 4 stages are complete (0 – 4).
        /// </summary>
        [NotMapped]
        public int CompletedStages =>
            (IsCastingDone        ? 1 : 0) +
            (IsFinishingTouchDone ? 1 : 0) +
            (IsGoldPlatingDone    ? 1 : 0) +
            (IsPackagingDone      ? 1 : 0);

        /// <summary>
        /// Completion percentage (0 %, 25 %, 50 %, 75 %, 100 %).
        /// </summary>
        [NotMapped]
        [Display(Name = "Completion %")]
        public int CompletionPercentage => CompletedStages * 25;

        /// <summary>
        /// True only when all 4 stages are done.
        /// </summary>
        [NotMapped]
        public bool IsComplete => CompletedStages == 4;

        /// <summary>
        /// Human-readable label for the next pending stage.
        /// </summary>
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
