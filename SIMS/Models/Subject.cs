using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIMS.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }

        [Required, StringLength(20)]
        public string Code { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        // navigation back to classes
        public ICollection<Class> Classes { get; set; }
    }
}
