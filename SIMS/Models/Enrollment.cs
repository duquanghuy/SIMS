using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        [Required]
        public string StudentId { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required]
        public DateTime EnrolledOn { get; set; }

        // Optional grade field—remove or change the type if you don’t need it
        [StringLength(5)]
        public string? Grade { get; set; }

        // Navigation properties
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }

        [ForeignKey(nameof(ClassId))]
        public Class Class { get; set; }
    }
}
