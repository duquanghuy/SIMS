using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SIMS.Models.ViewModels
{
    public class AdminViewScoresViewModel
    {
        public List<SelectListItem> Classes { get; set; } = new();
        public int? SelectedClassId { get; set; }
        public List<StudentScoreInfo> Students { get; set; } = new();
    }

    public class StudentScoreInfo
    {
        public string StudentName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Score { get; set; }
    }
} 