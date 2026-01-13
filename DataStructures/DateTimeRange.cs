using DataStructures.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class DateTimeRange
    {
        private DateTimeRange()
        {
        }

        public DateTimeRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTimeRange(DateTime start)
        {
            Start = start;
            End = start.AddDays(1).AddMilliseconds(-1);
        }

        public DateTime Start;
        public DateTime End;

        public List<DateOnly> GetDates()
        {
            List<DateOnly> allDates = new List<DateOnly>();
            DateOnly currentDate = DateOnly.FromDateTime(Start);
            DateOnly endDate = DateOnly.FromDateTime(End);
            while (currentDate <= endDate)
            {
                allDates.Add(currentDate);
                currentDate = currentDate.AddDays(1);
            }
            return allDates;
        }

        public List<DateTime> GetDateTimes()
        {
            List<DateTime> allDates = new List<DateTime>();
            var date = Start;
            while(date <= End)
            {
                allDates.Add(date);
                date = Start.AddDays(1);
            }
            return allDates;
        }

        public Dictionary<int,List<DateTime>> GetDateTimesByWeek(DayOfWeek firstDayOfWeek, bool adjustToWeekRange = true)
        {
            List<DateTime> allDates = new List<DateTime>();
            Start = adjustToWeekRange ? Start.FirstOftheWeek(firstDayOfWeek).ResetTime() : Start;
            End = adjustToWeekRange ? End.LastOfTheWeek(firstDayOfWeek) : End;

            Dictionary<int, List<DateTime>> datesByWeek = new Dictionary<int, List<DateTime>>();
            int weekCount = 0;
            var date = Start;
            while (date <= End)
            {
                if(date.DayOfWeek == End.DayOfWeek)
                    weekCount++;
                allDates.Add(date);
                if (date.DayOfWeek == End.DayOfWeek)
                {
                    datesByWeek.Add(weekCount, new List<DateTime>(allDates));
                    allDates.Clear();
                }
                date = date.AddDays(1);
            }

            return datesByWeek;
        }

        public Dictionary<int, List<DateOnly>> GetDatesByWeek(DayOfWeek firstDayOfWeek, bool adjustToWeekRange = true)
        {
            var dateTimesByWeek = GetDateTimesByWeek(firstDayOfWeek, adjustToWeekRange);
            Dictionary<int, List<DateOnly>> datesByWeek = new Dictionary<int, List<DateOnly>>();
            foreach(var kvp in dateTimesByWeek)
            {
                List<DateOnly> dateOnlyList = new List<DateOnly>();
                foreach(var dt in kvp.Value)
                {
                    dateOnlyList.Add(DateOnly.FromDateTime(dt));
                }
                datesByWeek.Add(kvp.Key, dateOnlyList);
            }
            return datesByWeek;
        }

        public (bool found, DateTimeRange weekRange) TrySliceWeekRange(DayOfWeek firstDayOfWeek, int weekNumber = 1)
        {
            if ((End - Start).TotalDays < 7)
            {
                return (false, this);
            }

            if (weekNumber == 1)
            {
                if ((End - Start).TotalDays >= 7)
                {
                    var firstDateOfTheWeek = Start.FirstOftheWeek(firstDayOfWeek).ResetTime();
                    var weekRange = new DateTimeRange(firstDateOfTheWeek, firstDateOfTheWeek.AddDays(7).AddMicroseconds(-1));
                    return (true, weekRange);
                }
            }
            if (weekNumber > 1)
            {
                DateTime startFrom = Start.AddDays(7 * (weekNumber - 1));
                if (startFrom <= End.FirstOftheWeek(firstDayOfWeek).AddDays(7).AddMicroseconds(-1))
                {
                    var firstDateOfTheWeek = startFrom.FirstOftheWeek(firstDayOfWeek).ResetTime();
                    var weekRange = new DateTimeRange(firstDateOfTheWeek, firstDateOfTheWeek.AddDays(7).AddMicroseconds(-1));
                    return (true, weekRange);
                }
            }

            return (false, this);
        }

        public bool IsInRange(DateTime date)
        {
            return date >= Start && date <= End;
        }

        public override string ToString()
        {
            return $"{Start.ToShortDateString()} - {End.ToShortDateString()}";
        }

        public static DateTimeRange GetWeekRange(DateTime targetDate, DayOfWeek firstDayOfWeek, int numberOfWeeks = 1)
        {
            var firstDateOfTheWeek = targetDate.FirstOftheWeek(firstDayOfWeek);
            firstDateOfTheWeek = firstDateOfTheWeek.ResetTime();
            return new DateTimeRange(firstDateOfTheWeek, firstDateOfTheWeek.AddDays(7*numberOfWeeks).AddMicroseconds(-1));
        }
        
        public static DateTimeRange GetMonthRange(DateTime targetDate)
        {
            DateTime firstDayOfMonth = targetDate.FirstOfTheMonth().ResetTime();
            DateTime lastDayOfMonth = firstDayOfMonth.LastOfTheMonth().ResetTime();
            return new DateTimeRange(firstDayOfMonth,lastDayOfMonth.AddDays(1).AddMicroseconds(-1));
        }

        public static DateTimeRange GetMonthRange(DateTime targetDate, int numberOfMonth)
        {
            DateTime firstDayOfMonth = targetDate.FirstOfTheMonth().ResetTime();
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(numberOfMonth-1).LastOfTheMonth().ResetTime();
            return new DateTimeRange(firstDayOfMonth, lastDayOfMonth.AddDays(1).AddMicroseconds(-1));
        }

        public static DateTimeRange GetYearRange(DateTime targetDate,int numberOfYears = 1)
        {
            DateTime firstDayOfYear = new DateTime(targetDate.Year, 1, 1).ResetTime();
            DateTime firstDayOfNextYear = firstDayOfYear.AddYears(numberOfYears);
            return new DateTimeRange(firstDayOfYear, firstDayOfNextYear.AddMicroseconds(-1));
        }
    }
}
