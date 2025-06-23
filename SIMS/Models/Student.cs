using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // manually set
        public string StudentId { get; set; }

        public int StudentNumber { get; set; } // manually generated now

        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime EnrollmentDate { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        public User User { get; set; }
    }

}
