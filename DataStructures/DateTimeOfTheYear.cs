using DataStructures.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class DateTimeOfTheYearRule
    {
        /// <summary>
        /// Month number (1–12). If null, matches all months in Match().
        /// For GetDate(year), if null, month 1 is assumed.
        /// </summary>
        public int? MonthNumber { get; set; }

        /// <summary>
        /// Specific day of month (1–31). If set, the date must match this day.
        /// </summary>
        public int? DayOfMonthNumber { get; set; }

        /// <summary>
        /// Week of month (1+). Uses GetWeekOfMonth() convention.
        /// </summary>
        public int? WeekOfMonthNumber { get; set; }
        public bool FirstWeekRule { get; set; } = false;
        public bool SecondWeekRule { get; set; } = false;
        /// <summary>
        /// True = must be the first day of the month.
        /// </summary>
        public bool? FirstOfMonth { get; set; }

        /// <summary>
        /// True = must be the last day of the month.
        /// </summary>
        public bool? LastOfMonth { get; set; }

        /// <summary>
        /// True = must be in the first week of the month (week == 1).
        /// </summary>
        public bool? FirstWeekOfMonth { get; set; }

        /// <summary>
        /// True = must be in the last week of the month
        /// (week == lastOfMonth.GetWeekOfMonth()).
        /// If DayOfWeek is set, this is also used for "last X of the month"
        /// via LastDayOfTheWeekInMonth.
        /// </summary>
        public bool? LastWeekOfMonth { get; set; }

        /// <summary>
        /// Day of week constraint (e.g. Friday).
        /// </summary>
        public DayOfWeek? DayOfWeekValue { get; set; }

        /// <summary>
        /// Nth occurrence of the DayOfWeek within the month (e.g. 4 = 4th Thursday).
        /// Requires DayOfWeek to be set to be meaningful.
        /// </summary>
        public int? DayOfWeekNumber { get; set; }

        /// <summary>
        /// Optional start time for the rule on a given matching date.
        /// If null, ToDateTimeRange will use 00:00.
        /// </summary>
        public TimeOnly? Start { get; set; }

        /// <summary>
        /// Optional end time for the rule on a given matching date.
        /// If null, ToDateTimeRange will use 23:59:59.
        /// </summary>
        public TimeOnly? End { get; set; }

        /// <summary>
        /// First day-of-week used by GetWeekOfMonth in this rule.
        /// Sunday by default (US-style weeks).
        /// </summary>
        public DayOfWeek FirstDayOfWeekForWeeks { get; set; } = DayOfWeek.Sunday;

        /// <summary>
        /// Returns one canonical date in the given year that matches this rule.
        /// Useful for patterns that naturally pick one date per year
        /// (e.g. "4th Thursday of November" for Thanksgiving).
        /// 
        /// If MonthNumber is null, month 1 is assumed.
        /// </summary>
        public DateOnly GetDate(int year, int? month = null)
        {
            int monthValue = month ?? MonthNumber ?? 1;
            var firstOfMonth = new DateOnly(year, monthValue, 1);

            // 1. Explicit: first / last day of month
            if (FirstOfMonth == true)
                return firstOfMonth;

            if (LastOfMonth == true)
                return firstOfMonth.LastOfTheMonth();

            // 2. Explicit: day-of-month
            if (DayOfMonthNumber.HasValue)
                return new DateOnly(year, monthValue, DayOfMonthNumber.Value);

            // 3. Day-of-week-based rules
            if (DayOfWeekValue.HasValue)
            {
                var dow = DayOfWeekValue.Value;

                // 3a. Nth occurrence of weekday in month (e.g. 4th Thursday)
                if (DayOfWeekNumber.HasValue && DayOfWeekNumber.Value > 0)
                {
                    return GetNthWeekdayOfMonth(year, monthValue, dow, DayOfWeekNumber.Value);
                }

                // 3b. LastWeekOfMonth + DayOfWeek => last such weekday in month
                if (LastWeekOfMonth == true)
                {
                    return GetLastWeekdayOfMonth(year, monthValue, dow);
                }

                // 3c. FirstWeekOfMonth + DayOfWeek => first such weekday in month
                if (FirstWeekOfMonth == true)
                {
                    return GetNthWeekdayOfMonth(year, monthValue, dow, 1);
                }

                // 3d. Specific week-of-month + DayOfWeek
                if (WeekOfMonthNumber.HasValue)
                {
                    var firstOccurrence = GetNthWeekdayOfMonth(year, monthValue, dow, 1);
                    var result = firstOccurrence.AddDays(7 * (WeekOfMonthNumber.Value - 1));
                    if (result.Month != monthValue)
                        throw new InvalidOperationException(
                            $"Week {WeekOfMonthNumber} with {dow} does not exist in {monthValue}/{year}.");
                    return result;
                }

                // 3e. Only DayOfWeek => first occurrence of that weekday in the month
                return GetNthWeekdayOfMonth(year, monthValue, dow, 1);
            }

            // 4. Fallback: first of month
            return firstOfMonth;
        }

        /// <summary>
        /// Returns all dates in the given year that satisfy this rule.
        /// Uses Match(DateOnly).
        /// </summary>
        public IEnumerable<DateOnly> GetDates(int year)
        {
            var start = new DateOnly(year, 1, 1);
            var end = new DateOnly(year, 12, 31);

            for (var date = start; date <= end; date = date.AddDays(1))
            {
                if (Match(date))
                    yield return date;
            }
        }

        /// <summary>
        /// Determines whether the given date satisfies this rule.
        /// All non-null properties are treated as constraints.
        /// </summary>
        public bool Match(DateOnly date)
        {
            // Month
            if (MonthNumber.HasValue && MonthNumber.Value != date.Month)
                return false;

            // First / last of month
            if (FirstOfMonth == true && date.Day != 1)
                return false;

            if (LastOfMonth == true && date.Day != date.LastOfTheMonth().Day)
                return false;

            // Day-of-month
            if (DayOfMonthNumber.HasValue && DayOfMonthNumber.Value != date.Day)
                return false;

            // Day-of-week
            if (DayOfWeekValue.HasValue && DayOfWeekValue.Value != date.DayOfWeek)
                return false;

            // Week-of-month constraints
            int? weekOfMonth = null;
            int? lastWeekOfMonth = null;

            if (WeekOfMonthNumber.HasValue || FirstWeekOfMonth == true || LastWeekOfMonth == true)
            {
                weekOfMonth = date.GetWeekOfMonth(FirstDayOfWeekForWeeks);
                lastWeekOfMonth = date
                    .LastOfTheMonth()
                    .GetWeekOfMonth(FirstDayOfWeekForWeeks);
            }

            // Specific week-of-month
            if (WeekOfMonthNumber.HasValue)
            {
                if (!weekOfMonth.HasValue || WeekOfMonthNumber.Value != weekOfMonth.Value)
                    return false;
            }

            // First week of month
            if (FirstWeekOfMonth == true)
            {
                if (!weekOfMonth.HasValue || weekOfMonth.Value != 1)
                    return false;
            }

            // Last week of month (generic, week number comparison)
            if (LastWeekOfMonth == true)
            {
                // If DayOfWeek is set, we treat this as "last X of month"
                // using LastDayOfTheWeekInMonth below, so we skip
                // the generic week check here and handle in a special case below.
            }

            // Special case: LastWeekOfMonth + DayOfWeek => last given weekday of month
            if (LastWeekOfMonth == true && DayOfWeekValue.HasValue)
            {
                var lastWeekday = date.LastDayOfTheWeekInMonth(DayOfWeekValue.Value);
                if (date != lastWeekday)
                    return false;
            }
            else if (LastWeekOfMonth == true && DayOfWeekValue == null)
            {
                // No specific DayOfWeek; just "some date in last week of month"
                if (!weekOfMonth.HasValue || !lastWeekOfMonth.HasValue ||
                    weekOfMonth.Value != lastWeekOfMonth.Value)
                {
                    return false;
                }
            }

            // Nth DayOfWeek in month (e.g. 2nd Wednesday)
            if (DayOfWeekNumber.HasValue)
            {
                if (!DayOfWeekValue.HasValue)
                    return false; // ambiguous usage

                int n = DayOfWeekNumber.Value;
                if (n <= 0)
                    return false;

                // Count occurrences of DayOfWeek up to and including this date
                int count = 0;
                var cursor = new DateOnly(date.Year, date.Month, 1);
                while (cursor.Month == date.Month && cursor <= date)
                {
                    if (cursor.DayOfWeek == DayOfWeekValue.Value)
                        count++;

                    cursor = cursor.AddDays(1);
                }

                if (count != n)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Create a DateTimeRange for a given date using Start/End times if set.
        /// If Start/End are null, uses 00:00 to 23:59:59 for that date.
        /// </summary>
        public DateTimeRange ToDateTimeRange(DateOnly date)
        {
            var startTime = Start ?? new TimeOnly(0, 0);
            var endTime = End ?? new TimeOnly(23, 59, 59);

            var start = date.ToDateTime(startTime);
            var end = date.ToDateTime(endTime);

            return new DateTimeRange(start, end);
        }

        public DateTimeRange ToDateTimeRange(DateTime dateTime)
        {
            var date = DateOnly.FromDateTime(dateTime);
            return ToDateTimeRange(date);
        }

        private static DateOnly GetNthWeekdayOfMonth(int year, int month, DayOfWeek dayOfWeek, int nth)
        {
            if (nth <= 0)
                throw new ArgumentOutOfRangeException(nameof(nth), "nth must be >= 1.");

            var firstOfMonth = new DateOnly(year, month, 1);
            int offset = ((int)dayOfWeek - (int)firstOfMonth.DayOfWeek + 7) % 7;
            var firstOccurrence = firstOfMonth.AddDays(offset);
            var result = firstOccurrence.AddDays(7 * (nth - 1));

            if (result.Month != month)
            {
                throw new InvalidOperationException(
                    $"The {nth} {dayOfWeek} does not exist in {month}/{year}.");
            }

            return result;
        }

        private static DateOnly GetLastWeekdayOfMonth(int year, int month, DayOfWeek dayOfWeek)
        {
            var lastOfMonth = new DateOnly(year, month, 1).LastOfTheMonth();
            int backOffset = ((int)lastOfMonth.DayOfWeek - (int)dayOfWeek + 7) % 7;
            return lastOfMonth.AddDays(-backOffset);
        }
    }
}
