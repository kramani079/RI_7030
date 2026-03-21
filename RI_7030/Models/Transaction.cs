using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RI_7030.Models
{
    /// <summary>
    /// Financial transaction — RI_3001, RI_3002, ...
    /// Type:     Buy | Sell
    /// Status:   Pending | Paid | Received | Cancelled
    /// Category: EMP_SALARY | EMP_ADVANCE | PRODUCT | EXPENSE
    /// </summary>
    public class Transaction
    {
        [Key]
        [StringLength(20)]
        public string TransactionId { get; set; } = string.Empty;

        /// <summary>Buy | Sell</summary>
        [Required]
        [StringLength(10)]
        public string Type { get; set; } = "Buy";

        [StringLength(150)]
        public string? PartyName { get; set; }

        [StringLength(20)]
        public string? ProductId { get; set; }

        [StringLength(150)]
        public string? ProductName { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        /// <summary>Pending | Paid | Received | Cancelled</summary>
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        public DateTime TransactionDate { get; set; } = DateTime.Now;

        /// <summary>EMP_SALARY | EMP_ADVANCE | PRODUCT | EXPENSE</summary>
        [StringLength(30)]
        public string? Category { get; set; }

        /// <summary>Cash | UPI | Bank Transfer | Pending</summary>
        [StringLength(30)]
        public string? PaymentMethod { get; set; }
    }
}
