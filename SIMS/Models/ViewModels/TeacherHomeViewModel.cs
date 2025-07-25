using System.Collections.Generic;

namespace SIMS.Models.ViewModels
{
    public class TeacherHomeViewModel
    {
        public List<TeacherTodaysClassInfo> TodaysClasses { get; set; } = new();
        public List<TeacherClassInfo> AllClasses { get; set; } = new();
        public double OverallAverageScore { get; set; }
    }

    public class TeacherTodaysClassInfo
    {
        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public string TimeSlot { get; set; }
    }

    public class TeacherClassInfo
    {
        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public int StudentCount { get; set; }
        public double AverageScore { get; set; }
    }
} 