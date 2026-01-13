using DataStructures;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net.Http.Headers;
using System.Text;

namespace DataStructuresTests
{
    [TestClass]
    public class DateTimeSchedulerTests
    {
        [TestMethod]
        public void DateTimeScheduler_SchedulesBiWeeklyEvents_Correctly()
        {
            // Arrange
            var scheduler = new DateTimeScheduler
            {
                RecurrenceFrequency = RecurrenceFrequency.BiWeekly,
                FirstDayOfTheWeek = DayOfWeek.Monday,
                DateTimeOfTheYearRules = new List<DateTimeOfTheYearRule>
                {
                    new DateTimeOfTheYearRule
                    {
                        DayOfWeekValue = DayOfWeek.Wednesday,
                        FirstDayOfWeekForWeeks = DayOfWeek.Monday
                    }
                }
            };
            var range = new DateTimeRange(
                new DateTime(2024, 1, 10),
                new DateTime(2024, 3, 31)
            );
            // Act
            var scheduledRanges = scheduler.GetDateTimeRanges(range);
            // Assert
            var expectedDates = new List<DateTime>
            {
                new DateTime(2024, 1, 10), // Second Wednesday of January
                new DateTime(2024, 1, 24), // Two weeks later
                new DateTime(2024, 2, 7),  // Two weeks later
                new DateTime(2024, 2, 21), // Two weeks later
                new DateTime(2024, 3, 6),  // Two weeks later
                new DateTime(2024, 3, 20)  // Two weeks later
            };

            Assert.HasCount(expectedDates.Count, scheduledRanges);
            for (int i = 0; i < expectedDates.Count; i++)
            {
                Assert.AreEqual(expectedDates[i], scheduledRanges[i].Start);
            }
        }


        [TestMethod]
        public void DateTimeScheduler_SchedulesWeeklyEvents_Correctly()
        {
            // Arrange
            var scheduler = new DateTimeScheduler
            {
                RecurrenceFrequency = RecurrenceFrequency.Weekly,
                FirstDayOfTheWeek = DayOfWeek.Monday,
                DateTimeOfTheYearRules = new List<DateTimeOfTheYearRule>
                {
                    new DateTimeOfTheYearRule
                    {
                        DayOfWeekValue = DayOfWeek.Tuesday,
                        FirstDayOfWeekForWeeks = DayOfWeek.Monday
                    }
                }
            };
            var range = DateTimeRange.GetMonthRange(new DateTime(2025,12,1));

            var scheduledRanges = scheduler.GetDateTimeRanges(range);

           Assert.HasCount(5,scheduledRanges);
        }

        [TestMethod]
        public void DateTimeScheduler_Schedules5DayWorkWeek_Correctly()
        {
            var scheduler = new DateTimeScheduler
            {
                AdjustToFullWeeks = false,
                RecurrenceFrequency = RecurrenceFrequency.Weekly,
                FirstDayOfTheWeek = DayOfWeek.Monday,
                DateTimeOfTheYearRules = new List<DateTimeOfTheYearRule>
                {
                    new DateTimeOfTheYearRule
                    {
                        DayOfWeekValue = DayOfWeek.Monday,
                        Start = new TimeOnly(8,0),
                        End = new TimeOnly(5,0),
                        FirstDayOfWeekForWeeks = DayOfWeek.Monday
                    },
                    new DateTimeOfTheYearRule
                    {
                        DayOfWeekValue = DayOfWeek.Tuesday,
                        Start = new TimeOnly(8,0),
                        End = new TimeOnly(5,0),
                        FirstDayOfWeekForWeeks = DayOfWeek.Monday
                    },
                    new DateTimeOfTheYearRule
                    {
                        DayOfWeekValue = DayOfWeek.Wednesday,
                        Start = new TimeOnly(8,0),
                        End = new TimeOnly(5,0),
                        FirstDayOfWeekForWeeks = DayOfWeek.Monday
                    }
                    ,new DateTimeOfTheYearRule
                    {
                        DayOfWeekValue = DayOfWeek.Thursday,
                        Start = new TimeOnly(8,0),
                        End = new TimeOnly(5,0),
                        FirstDayOfWeekForWeeks = DayOfWeek.Monday
                    },
                    new DateTimeOfTheYearRule
                    {
                        DayOfWeekValue = DayOfWeek.Friday,
                        Start = new TimeOnly(8,0),
                        End = new TimeOnly(5,0),
                        FirstDayOfWeekForWeeks = DayOfWeek.Monday
                    }
                }
                
            };

            var december2025 = new DateTime(2025, 12, 1);
            var range = DateTimeRange.GetMonthRange(december2025);

            var scheduledRanges = scheduler.GetDateTimeRanges(range);

            Assert.HasCount(23, scheduledRanges);
        }

        [TestMethod]
        public void DateTimeScheduler_Schedules4DayWorkWeek_Correctly()
        {
            var scheduler = new DateTimeScheduler
            {
                AdjustToFullWeeks = false,
                RecurrenceFrequency = RecurrenceFrequency.BiWeekly,
                FirstDayOfTheWeek = DayOfWeek.Monday,
                
                DateTimeOfTheYearRules = new List<DateTimeOfTheYearRule>
                {
                    new DateTimeOfTheYearRule
                    {
                        FirstWeekRule = true,
                        DayOfWeekValue = DayOfWeek.Tuesday,
                        Start = new TimeOnly(8,0),
                        End = new TimeOnly(5,0),
                        FirstDayOfWeekForWeeks = DayOfWeek.Monday
                    },
                    new DateTimeOfTheYearRule
                    {
                        FirstWeekRule = true,
                        DayOfWeekValue = DayOfWeek.Wednesday,
                        Start = new TimeOnly(8,0),
                        End = new TimeOnly(5,0),
                        FirstDayOfWeekForWeeks = DayOfWeek.Monday
                    }
                    ,new DateTimeOfTheYearRule
                    {
                        FirstWeekRule = true,
                        DayOfWeekValue = DayOfWeek.Thursday,
                        Start = new TimeOnly(8,0),
                        End = new TimeOnly(5,0),
                        FirstDayOfWeekForWeeks = DayOfWeek.Monday
                    },
                    new DateTimeOfTheYearRule
                    {
                        FirstWeekRule = true,
                        DayOfWeekValue = DayOfWeek.Friday,
                        Start = new TimeOnly(8,0),
                        End = new TimeOnly(5,0),
                        FirstDayOfWeekForWeeks = DayOfWeek.Monday
                    },

                    new DateTimeOfTheYearRule
                    {
                        SecondWeekRule = true,
                        DayOfWeekValue = DayOfWeek.Monday,
                        Start = new TimeOnly(8,0),
                        End = new TimeOnly(5,0),
                        FirstDayOfWeekForWeeks = DayOfWeek.Monday
                    },
                    new DateTimeOfTheYearRule
                    {
                        SecondWeekRule = true,
                        DayOfWeekValue = DayOfWeek.Tuesday,
                        Start = new TimeOnly(8,0),
                        End = new TimeOnly(5,0),
                        FirstDayOfWeekForWeeks = DayOfWeek.Monday
                    },
                    new DateTimeOfTheYearRule
                    {
                        SecondWeekRule = true,
                        DayOfWeekValue = DayOfWeek.Wednesday,
                        Start = new TimeOnly(8,0),
                        End = new TimeOnly(5,0),
                        FirstDayOfWeekForWeeks = DayOfWeek.Monday
                    }
                    ,new DateTimeOfTheYearRule
                    {
                        SecondWeekRule = true,
                        DayOfWeekValue = DayOfWeek.Thursday,
                        Start = new TimeOnly(8,0),
                        End = new TimeOnly(5,0),
                        FirstDayOfWeekForWeeks = DayOfWeek.Monday
                    },
                    
                }

            };

            var december2025 = new DateTime(2025, 12, 1);
            var range = DateTimeRange.GetMonthRange(december2025);

            var scheduledRanges = scheduler.GetDateTimeRanges(range);

            Assert.HasCount(18, scheduledRanges);
        }
    }
}
