using System.ComponentModel.DataAnnotations;

namespace RI_7030.Models
{
    public enum TransactionStatus
    {
        Pending,
        Paid,
        Cancelled,
        Refunded
    }

    public enum PaymentMethod
    {
        Cash,
        CreditCard,
        DebitCard,
        BankTransfer,
        Online
    }

    public class Transaction
    {
        public int TransactionId { get; set; }

        [Required]
        [Display(Name = "Transaction Date")]
        [DataType(DataType.DateTime)]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(150)]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(200)]
        [Display(Name = "Customer Email")]
        public string? CustomerEmail { get; set; }

        // Foreign key to InventoryItem
        [Display(Name = "Item")]
        public int? ItemId { get; set; }

        [StringLength(150)]
        [Display(Name = "Product / Item")]
        public string? ItemName { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0, 10_000_000)]
        [Display(Name = "Unit Price")]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Total Amount")]
        [DataType(DataType.Currency)]
        public decimal TotalAmount => Quantity * UnitPrice;

        [Required]
        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

        [Required]
        [Display(Name = "Payment Method")]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
