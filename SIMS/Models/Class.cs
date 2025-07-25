using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SIMS.Models;

namespace SIMS.Models
{ 
    public class Class
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassId { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public string TeacherId { get; set; }

        public Subject Subject { get; set; }
        public Teacher Teacher { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
                = new List<Enrollment>();

        public ICollection<ClassSchedule> ClassSchedules { get; set; }
        = new List<ClassSchedule>();
    }
}
