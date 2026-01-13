using System;
using System.Collections.Generic;
using System.Text;

namespace TeamRoster.Domain.Entities
{
    public class ShiftBlockc
    {
        public List<DateOnly> Days { get; set; } = new List<DateOnly>();
        public List<WorkDayOfTheWeek> DaysOfFirstWeek { get; set; } = new List<WorkDayOfTheWeek>();
        public RecurrenceFrequency RecurrenceFrequency { get; set; }

    }

    public class WorkDayOfTheWeek
    {
        public int WeekNumber { get; set; }
        DayOfWeek DayOfWeek { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
    } 

    public enum RecurrenceFrequency { None, Daily, ByWeekly, Weekly, Monthly, Yearly }
}
