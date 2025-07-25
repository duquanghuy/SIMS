using System;
using System.Collections.Generic;
using SIMS.Models;

namespace SIMS.Models.ViewModels
{
    public class TeacherScheduleViewModel
    {
        public Dictionary<DayOfWeek, Dictionary<string, List<ClassSchedule>>> Slots { get; set; }

        public TeacherScheduleViewModel()
        {
            Slots = new Dictionary<DayOfWeek, Dictionary<string, List<ClassSchedule>>>();
            foreach (DayOfWeek d in Enum.GetValues(typeof(DayOfWeek)))
            {
                Slots[d] = new Dictionary<string, List<ClassSchedule>>
                {
                    ["Morning"] = new List<ClassSchedule>(),
                    ["Afternoon"] = new List<ClassSchedule>()
                };
            }
        }
    }
} 