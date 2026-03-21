using System.ComponentModel.DataAnnotations;

namespace RI_7030.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>'Admin' | 'Employee'</summary>
        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "Employee";

        /// <summary>'Casting' | 'Finishing Touch' | 'Gold Plating' | 'Packaging'</summary>
        [StringLength(50)]
        public string? EmployeeType { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(300)]
        public string? Bio { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        [StringLength(100)]
        public string? CityState { get; set; }

        [StringLength(20)]
        public string? PostalCode { get; set; }

        [StringLength(50)]
        public string? TaxId { get; set; }

        [StringLength(300)]
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
