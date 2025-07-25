using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS.Models
{
    public class ClassSchedule
    {
        [Key]
        public int ScheduleId { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required, StringLength(10)]
        public string DayOfWeek { get; set; }

        [Required, StringLength(10)]
        public string TimeSlot { get; set; }

        // Navigation back to Class
        [ForeignKey(nameof(ClassId))]
        public Class Class { get; set; }
    }
}
