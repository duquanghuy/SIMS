using System.Collections.Generic;

namespace SIMS.Models.ViewModels
{
    public class StudentScoreViewModel
    {
        public List<StudentScoreRow> Scores { get; set; } = new();
    }

    public class StudentScoreRow
    {
        public string ClassName { get; set; }
        public string TeacherName { get; set; }
        public string TeacherEmail { get; set; }
        public string Score { get; set; }
    }
} 