using System;
using System.ComponentModel.DataAnnotations;

namespace SIMS.Models.ViewModels
{
    public class StudentProfileViewModel
    {
        public string StudentId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public DateTime EnrollmentDate { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address must be under 200 characters.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string? PhoneNumber { get; set; }

        // ➕ Thông tin đổi mật khẩu
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "New password must be at least 8 characters.")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password confirmation does not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
