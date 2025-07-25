using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIMS.Models;

namespace SIMS.Models.ViewModels
{
    public class CalendarViewModel
    {
        // Which week to show (0 = this week, ±1 for next/prev)
        public int WeekOffset { get; set; }

        // 7 days × 2 slots
        public Dictionary<DayOfWeek, Dictionary<string, List<ClassSchedule>>> Slots { get; set; }

        // Dropdown of classes
        public SelectList AllClasses { get; set; }

        // For your Assign form
        [Required] public int SelectedClassId { get; set; }
        [Required] public DayOfWeek SelectedDay { get; set; }
        [Required] public string SelectedTimeSlot { get; set; }

        public CalendarViewModel()
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
