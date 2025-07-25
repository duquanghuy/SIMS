using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SIMS.Models.ViewModels
{
    public class ClassViewModel
    {
        public int? ClassId { get; set; }

        [Required(ErrorMessage = "Please select a subject.")]
        [Display(Name = "Subject")]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Please select a teacher.")]
        [Display(Name = "Teacher")]
        public string TeacherId { get; set; }

        // For dropdown lists
        [ValidateNever]
        public SelectList Subjects { get; set; }
        [ValidateNever]
        public SelectList Teachers { get; set; }
    }
}
