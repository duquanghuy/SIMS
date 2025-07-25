using System;
using System.Collections.Generic;

namespace SIMS.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalClasses { get; set; }
        public int TotalSubjects { get; set; }
        public int ActiveEnrollments { get; set; }
        public double AverageScore { get; set; }

        public List<ClassCount> StudentsPerClass { get; set; } = new();
        public List<SubjectCount> StudentsBySubject { get; set; } = new();
        public List<EnrollmentTrend> EnrollmentTrends { get; set; } = new();
        public List<ClassScore> AverageScorePerClass { get; set; } = new();
    }

    public class ClassCount
    {
        public string ClassLabel { get; set; }
        public int Count { get; set; }
    }
    public class SubjectCount
    {
        public string SubjectLabel { get; set; }
        public int Count { get; set; }
    }
    public class EnrollmentTrend
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
    public class ClassScore
    {
        public string ClassLabel { get; set; }
        public double AverageScore { get; set; }
    }
} 