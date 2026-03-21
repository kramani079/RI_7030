using System.ComponentModel.DataAnnotations;

namespace RI_7030.Models
{
    /// <summary>
    /// Employee record — string PK: RI_4001, RI_4002, ...
    /// UserId links to a User account (nullable).
    /// </summary>
    public class Employee
    {
        [Key]
        [StringLength(20)]
        public string EmployeeId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employee name is required.")]
        [StringLength(150)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [StringLength(150)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>Casting | Finishing Touch | Gold Plating | Packaging</summary>
        [StringLength(50)]
        [Display(Name = "Department")]
        public string? Department { get; set; }

        [Range(0, 10_000_000)]
        public decimal Salary { get; set; }

        /// <summary>Foreign key to Users table (nullable — employee may or may not have login)</summary>
        public int? UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
