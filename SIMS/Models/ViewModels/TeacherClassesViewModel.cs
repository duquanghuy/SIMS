using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SIMS.Models.ViewModels
{
    public class TeacherClassesViewModel
    {
        public List<SelectListItem> Classes { get; set; } = new();
        public int? SelectedClassId { get; set; }
        public List<StudentInClassInfo> Students { get; set; } = new();
    }

    public class StudentInClassInfo
    {
        public string StudentCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Score { get; set; }
    }
} 