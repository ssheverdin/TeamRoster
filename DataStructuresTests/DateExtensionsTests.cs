using DataStructures.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructuresTests
{
    [TestClass]
    public class DateExtensionsTests
    {
        

        public DateExtensionsTests()
        {
        }

        [TestMethod]
        public void Date_GetWeekOfMonth_3()
        {
            DateTime targetDate = new DateTime(2025, 10, 17);
            int weekOfMonth = targetDate.GetWeekOfMonth();
            Assert.AreEqual(3, weekOfMonth);
        }

        [TestMethod]
        public void Date_GetWeekOfMonth_2()
        {
            DateTime targetDate = new DateTime(2025, 12, 11);
            int weekOfMonth = targetDate.GetWeekOfMonth();
            Assert.AreEqual(2, weekOfMonth);
        }

        [TestMethod]
        public void Date_GetWeekOfMonth_1()
        {
            DateTime targetDate = new DateTime(2025, 12, 1);
            int weekOfMonth = targetDate.GetWeekOfMonth();
            Assert.AreEqual(1, weekOfMonth);
        }

        [TestMethod]
        public void Date_GetWeekOfMonth_5()
        {
            DateTime targetDate = new DateTime(2025, 10, 31);
            int weekOfMonth = targetDate.GetWeekOfMonth();
            Assert.AreEqual(5, weekOfMonth);
        }

        public void Date_FirstOftheWeek()
        {
            DateOnly targetDate = new DateOnly(2025, 10, 17); // Friday
            DateOnly firstOfWeek = targetDate.FirstOftheWeek(DayOfWeek.Monday);
            Assert.AreEqual(new DateOnly(2025, 10, 13), firstOfWeek);
        }

        public void Date_LastOftheWeek()
        {
            DateOnly targetDate = new DateOnly(2025, 10, 17); // Friday
            DateOnly lastOfWeek = targetDate.LastOfTheWeek(DayOfWeek.Monday);
            Assert.AreEqual(new DateOnly(2025, 10, 19), lastOfWeek);
        }
    }
}
