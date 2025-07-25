using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIMS.Models;

namespace SIMS.Models.ViewModels
{
    public class EnrollmentViewModel
    {
        public int ClassId { get; set; }
        public string ClassDisplay { get; set; }
        public IEnumerable<Enrollment> Enrolled { get; set; }

        [ValidateNever]
        public SelectList Available { get; set; }

        [Required(ErrorMessage = "Please select one or more students.")]
        public List<string> SelectedStudentIds { get; set; } = new();
    }
}
