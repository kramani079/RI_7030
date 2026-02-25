using System.ComponentModel.DataAnnotations;

namespace RI_7030.Models
{
    public enum EmployeeType
    {
        Casting = 1,
        FinishingTouch = 2,
        GoldPlating = 3,
        Packaging = 4
    }

    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Employee name is required.")]
        [StringLength(150)]
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [Display(Name = "Email ID")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employee type is required.")]
        [Display(Name = "Type of Employee")]
        public EmployeeType EmployeeType { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        [Range(0, 10_000_000, ErrorMessage = "Enter a valid salary.")]
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Phone]
        [Display(Name = "Mobile No.")]
        public string? Phone { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of Joining")]
        public DateTime DateOfJoining { get; set; } = DateTime.Now;
    }
}
