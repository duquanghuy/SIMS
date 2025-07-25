using System.Collections.Generic;

namespace SIMS.Models.ViewModels
{
    public class StudentHomeViewModel
    {
        public List<TodaysClassInfo> TodaysClasses { get; set; } = new();
        public List<ClassScoreInfo> AllClasses { get; set; } = new();
        public double GPA { get; set; }
    }

    public class TodaysClassInfo
    {
        public string ClassName { get; set; }
        public string TimeSlot { get; set; }
    }

    public class ClassScoreInfo
    {
        public string ClassName { get; set; }
        public string TeacherName { get; set; }
        public string TeacherEmail { get; set; }
        public string Score { get; set; }
    }
} 