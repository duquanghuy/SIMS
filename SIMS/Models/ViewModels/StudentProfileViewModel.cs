using System;
using System.ComponentModel.DataAnnotations;

namespace SIMS.Models.ViewModels
{
    public class StudentProfileViewModel
    {
        public string StudentId { get; set; } // Mã sinh viên

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public DateTime EnrollmentDate { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address must be less than 200 characters")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
        public string? PhoneNumber { get; set; }
    }
}
