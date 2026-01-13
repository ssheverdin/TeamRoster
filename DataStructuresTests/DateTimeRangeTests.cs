using DataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructuresTests
{
    [TestClass]
    public class DateTimeRangeTests
    {
        [TestMethod]
        public void DateTimeRange_GetDates_MultipleDays()
        {
            DateTime start = new DateTime(2025, 10, 15);
            DateTime end = new DateTime(2025, 10, 18);
            DateTimeRange range = new DateTimeRange(start, end);
            List<DateOnly> dates = range.GetDates();
            Assert.AreEqual(4, dates.Count);
            Assert.AreEqual(new DateOnly(2025, 10, 15), dates[0]);
            Assert.AreEqual(new DateOnly(2025, 10, 16), dates[1]);
            Assert.AreEqual(new DateOnly(2025, 10, 17), dates[2]);
            Assert.AreEqual(new DateOnly(2025, 10, 18), dates[3]);
        }

        [TestMethod]
        public void DateTimeRange_IsInRange_Test()
        {
            DateTime start = new DateTime(2025, 10, 15);
            DateTime end = new DateTime(2025, 10, 18);
            DateTimeRange range = new DateTimeRange(start, end);
            Assert.IsTrue(range.IsInRange(new DateTime(2025, 10, 15)));
            Assert.IsTrue(range.IsInRange(new DateTime(2025, 10, 16)));
            Assert.IsTrue(range.IsInRange(new DateTime(2025, 10, 17)));
            Assert.IsTrue(range.IsInRange(new DateTime(2025, 10, 18)));
            Assert.IsFalse(range.IsInRange(new DateTime(2025, 10, 14)));
            Assert.IsFalse(range.IsInRange(new DateTime(2025, 10, 19)));
        }

        [TestMethod]
        public void DateTimeRange_GetsDateForWeek()
        {
            DateTime friday = new DateTime(2025, 12, 12); // Friday
            DateTime monday = new DateTime(2025, 12, 8);
            DateTime sunday = new DateTime(2025, 12, 14);

            DateTimeRange range =  DateTimeRange.GetWeekRange(friday, DayOfWeek.Monday);

            Assert.AreEqual(DayOfWeek.Monday, range.Start.DayOfWeek);
            Assert.AreEqual(monday.Date, range.Start.Date);
            Assert.AreEqual(DayOfWeek.Sunday, range.End.DayOfWeek);
            Assert.AreEqual(sunday.Date, range.End.Date);
        }

        [TestMethod]
        public void DateTimeRange_GetsDateForWeek_SameDay()
        {
            DateTime monday = new DateTime(2025, 12, 8);
            DateTime sunday = new DateTime(2025, 12, 14);
            DateTimeRange range = DateTimeRange.GetWeekRange(monday, DayOfWeek.Monday);
            Assert.AreEqual(DayOfWeek.Monday, range.Start.DayOfWeek);
            Assert.AreEqual(monday.Date, range.Start.Date);
            Assert.AreEqual(DayOfWeek.Sunday, range.End.DayOfWeek);
            Assert.AreEqual(sunday.Date, range.End.Date);
        }

        [TestMethod]
        public void DateTimeRange_GetsDateForWeeks()
        {
            DateTime friday = new DateTime(2025, 12, 12);
            DateTime monday = new DateTime(2025, 12, 8);
            DateTime forthWeekSunday = new DateTime(2026, 1, 4);

            DateTimeRange range = DateTimeRange.GetWeekRange(friday, DayOfWeek.Monday, 4);

            Assert.AreEqual(DayOfWeek.Monday, range.Start.DayOfWeek);
            Assert.AreEqual(monday.Date, range.Start.Date);
            Assert.AreEqual(DayOfWeek.Sunday, range.End.DayOfWeek);
            Assert.AreEqual(forthWeekSunday.Date, range.End.Date);
        }

        [TestMethod]
        public void DateTimeRange_GetsMonth()
        {
            DateTime friday = new DateTime(2025, 12, 12);
            DateTime firstDateOfMonth = new DateTime(2025, 12, 1);
            DateTime lastDateOfMonth = new DateTime(2025, 12, 31);

            DateTimeRange range = DateTimeRange.GetMonthRange(friday);


            Assert.AreEqual(firstDateOfMonth.Date, range.Start.Date);
            Assert.AreEqual(lastDateOfMonth.Date, range.End.Date);
        }

        [TestMethod]
        public void DateTimeRange_Months()
        {
            DateTime friday = new DateTime(2025, 12, 12);
            DateTime firstDateOfMonth = new DateTime(2025, 12, 1);
            DateTime lastDateOf4thMonth = new DateTime(2026, 2, 28);

            DateTimeRange range = DateTimeRange.GetMonthRange(friday,3);


            Assert.AreEqual(firstDateOfMonth.Date, range.Start.Date);
            Assert.AreEqual(lastDateOf4thMonth.Date, range.End.Date);
        }

        [TestMethod]
        public void DateTimeRange_ParsesWeeks()
        {
            int weekNumber = 3;
            DateTime friday = new DateTime(2025, 12, 12);
            DateTimeRange range = DateTimeRange.GetWeekRange(friday, DayOfWeek.Monday, weekNumber);
            var daysByWeek = range.GetDateTimesByWeek(DayOfWeek.Monday,adjustToWeekRange: true);
            
            Assert.HasCount(weekNumber, daysByWeek);
            foreach (var week in daysByWeek)
            {
                Assert.HasCount(7, week.Value);
            }
        }
    
    }

}
