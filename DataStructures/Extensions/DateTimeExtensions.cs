using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ResetTime(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, date.Kind);
        }

        public static int GetWeekOfMonth(this DateTime date)
        {
            var firstOfMonth = new DateTime(date.Year, date.Month, 1);
            int firstDayOfWeek = (int)firstOfMonth.DayOfWeek;

            if (firstDayOfWeek == 0)
                firstDayOfWeek = 7;

            return (date.Day + firstDayOfWeek - 1) / 7 + 1;
        }

        public static int GetWeekOfMonth(this DateTime date, DayOfWeek weekStart)
        {
            var firstOfMonth = new DateTime(date.Year, date.Month, 1);
            int firstDayOfWeek = (int)weekStart;

            return (date.Day + firstDayOfWeek - 1) / 7 + 1;
        }

        public static int GetWeekOfMonth(this DateOnly date)
        {
            return GetWeekOfMonth(date.ToDateTime(TimeOnly.MinValue));
        }

        public static DateOnly FirstOfTheMonth(this DateOnly date)
        {
            return new DateOnly(date.Year, date.Month, 1);
        }

        public static DateOnly LastOfTheMonth(this DateOnly date)
        {
            var firstOfNextMonth = date.Month == 12
                ? new DateOnly(date.Year + 1, 1, 1)
                : new DateOnly(date.Year, date.Month + 1, 1);
            return firstOfNextMonth.AddDays(-1);
        }

        public static DateTime FirstOfTheMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime LastOfTheMonth(this DateTime date)
        {
            var firstOfNextMonth = date.Month == 12
                ? new DateTime(date.Year + 1, 1, 1)
                : new DateTime(date.Year, date.Month + 1, 1);
            return firstOfNextMonth.AddDays(-1);
        }

        public static DateTime WithTime(this DateTime date, TimeOnly time)
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second, time.Millisecond, date.Kind);
        }

        public static DateOnly FirstOftheWeek(this DateOnly date, DayOfWeek weekStart)
        {
            int diff = (7 + (date.DayOfWeek - weekStart)) % 7;
            return date.AddDays(-1 * diff);
        }

        public static DateTime FirstOftheWeek(this DateTime date, DayOfWeek weekStart)
        {
            int diff = (7 + (date.DayOfWeek - weekStart)) % 7;
            return date.AddDays(-1 * diff);
        }

        public static DateTime LastOfTheWeek(this DateTime date, DayOfWeek weekStart)
        {
            return date.FirstOftheWeek(weekStart).ResetTime().AddDays(7).AddMicroseconds(-1);
        }

        public static DateOnly LastOfTheWeek(this DateOnly date, DayOfWeek weekStart)
        {
            return date.FirstOftheWeek(weekStart).AddDays(6);
        }

        public static DateTime LastDayOfTheWeekInMonth(this DateTime date, DayOfWeek dayOfWeek)
        {
            DateTime day =  date.LastOfTheMonth();
            while(day.DayOfWeek != dayOfWeek)
            {
                day = day.AddDays(-1);
            }
            return day;
        }

        public static DateOnly LastDayOfTheWeekInMonth(this DateOnly date, DayOfWeek dayOfWeek)
        {
            DateOnly day = date.LastOfTheMonth();
            while (day.DayOfWeek != dayOfWeek)
            {
                day = day.AddDays(-1);
            }
            return day;
        }

        /// <summary>
        /// Week of month, 1-based. Default first day of week = Sunday.
        /// </summary>
        public static int GetWeekOfMonth(this DateOnly date, DayOfWeek firstDayOfWeek = DayOfWeek.Sunday)
        {
            var firstOfMonth = new DateOnly(date.Year, date.Month, 1);

            int firstDow = (int)firstOfMonth.DayOfWeek;
            int targetFirst = (int)firstDayOfWeek;

            // How many days from the "calendar week's" start until the 1st of the month
            int offset = (7 + firstDow - targetFirst) % 7;

            int dayIndex = offset + (date.Day - 1);
            return (dayIndex / 7) + 1;
        }
    }

}
