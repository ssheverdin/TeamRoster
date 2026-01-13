using DataStructures;
using DataStructures.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructuresTests
{
    [TestClass]
    public class DateTimeOfTheYearTests
    {
        [TestMethod]
        public void ThanksgivingRule_ReturnsCorrectDates_2020_2030()
        {
            var rule = new DateTimeOfTheYearRule
            {
                MonthNumber = 11,                 // November
                DayOfWeekValue = DayOfWeek.Thursday,   // Thursday
                DayOfWeekNumber = 4               // 4th
            };

            var expected =
                new[]
                {
                    new { Year = 2020, Date = new DateOnly(2020, 11, 26) },
                    new { Year = 2021, Date = new DateOnly(2021, 11, 25) },
                    new { Year = 2022, Date = new DateOnly(2022, 11, 24) },
                    new { Year = 2023, Date = new DateOnly(2023, 11, 23) },
                    new { Year = 2024, Date = new DateOnly(2024, 11, 28) },
                    new { Year = 2025, Date = new DateOnly(2025, 11, 27) },
                    new { Year = 2026, Date = new DateOnly(2026, 11, 26) },
                    new { Year = 2027, Date = new DateOnly(2027, 11, 25) },
                    new { Year = 2028, Date = new DateOnly(2028, 11, 23) },
                    new { Year = 2029, Date = new DateOnly(2029, 11, 22) },
                    new { Year = 2030, Date = new DateOnly(2030, 11, 28) }
                };

            foreach (var e in expected)
            {
                var thanksgiving = rule.GetDate(e.Year);
                Assert.AreEqual(e.Date, thanksgiving,
                    $"Thanksgiving {e.Year} should be {e.Date}, but was {thanksgiving}.");

                // Also check GetDates(year) + Match
                var allMatches = rule.GetDates(e.Year).ToList();
                Assert.AreEqual(1, allMatches.Count,
                    $"There should be exactly 1 match for Thanksgiving in {e.Year}.");
                Assert.AreEqual(e.Date, allMatches[0],
                    $"GetDates({e.Year}) should contain only {e.Date}.");
            }
        }

        [TestMethod]
        public void LastOfMonthRule_ReturnsLastDayOfEachMonth()
        {
            var rule = new DateTimeOfTheYearRule
            {
                LastOfMonth = true
                // MonthNumber null → applies to all months
            };

            int year = 2024; // leap year to test Feb 29
            var dates = rule.GetDates(year).OrderBy(d => d).ToList();

            Assert.AreEqual(12, dates.Count, "Should have one date per month.");

            // Spot check a few
            Assert.AreEqual(new DateOnly(year, 1, 31), dates[0], "January last day mismatch.");
            Assert.AreEqual(new DateOnly(year, 2, 29), dates[1], "February last day mismatch.");
            Assert.AreEqual(new DateOnly(year, 4, 30), dates[3], "April last day mismatch.");
            Assert.AreEqual(new DateOnly(year, 12, 31), dates[11], "December last day mismatch.");

            // All should be the last day of their respective month
            foreach (var d in dates)
            {
                var expectedLast = d.LastOfTheMonth();
                Assert.AreEqual(expectedLast, d,
                    $"Date {d} should be last of month {d.Month} {d.Year}, but LastOfTheMonth is {expectedLast}.");
            }
        }

        [TestMethod]
        public void FirstMondayOfEachMonth_ReturnsCorrectDates()
        {
            var rule = new DateTimeOfTheYearRule
            {
                DayOfWeekValue = DayOfWeek.Monday,
                DayOfWeekNumber = 1 // first Monday
                // MonthNumber null → all months
            };

            int year = 2025;
            var dates = rule.GetDates(year).OrderBy(d => d).ToList();

            Assert.AreEqual(12, dates.Count, "Should have one first Monday per month.");

            foreach (var d in dates)
            {
                Assert.AreEqual(DayOfWeek.Monday, d.DayOfWeek, $"Date {d} should be Monday.");

                // First Monday must be day 1–7
                Assert.IsTrue(d.Day >= 1 && d.Day <= 7,
                    $"First Monday of month {d.Month}/{d.Year} should be within days 1–7, but was {d.Day}.");

                // There should be no earlier Monday in the same month
                var earlier = new DateOnly(d.Year, d.Month, 1);
                while (earlier < d)
                {
                    Assert.AreNotEqual(DayOfWeek.Monday, earlier.DayOfWeek,
                        $"Date {d} is not the first Monday; {earlier} is also Monday.");
                    earlier = earlier.AddDays(1);
                }
            }
        }

        [TestMethod]
        public void Match_WorksWithSimpleFixedDayRule()
        {
            var rule = new DateTimeOfTheYearRule
            {
                MonthNumber = 7,
                DayOfMonthNumber = 4
            };

            var matching = new DateOnly(2025, 7, 4);
            var nonMatchingMonth = new DateOnly(2025, 8, 4);
            var nonMatchingDay = new DateOnly(2025, 7, 5);

            Assert.IsTrue(rule.Match(matching), "7/4/2025 should match 4th of July rule.");
            Assert.IsFalse(rule.Match(nonMatchingMonth), "Should not match if month is different.");
            Assert.IsFalse(rule.Match(nonMatchingDay), "Should not match if day is different.");
        }

        [TestMethod]
        public void SecondWednesdayOfEachMonth_ReturnsCorrectDates()
        {
            var rule = new DateTimeOfTheYearRule
            {
                DayOfWeekValue = DayOfWeek.Wednesday,
                DayOfWeekNumber = 2 // 2nd Wednesday in each month
                                    // MonthNumber = null → applies to all months
            };

            int year = 2025;

            var dates = rule.GetDates(year)
                            .OrderBy(d => d)
                            .ToList();

            // Should be exactly one 2nd Wednesday per month
            Assert.AreEqual(12, dates.Count, "Should have exactly 12 dates (one per month).");

            var expected = new[]
            {
                new DateOnly(2025,  1,  8),
                new DateOnly(2025,  2, 12),
                new DateOnly(2025,  3, 12),
                new DateOnly(2025,  4,  9),
                new DateOnly(2025,  5, 14),
                new DateOnly(2025,  6, 11),
                new DateOnly(2025,  7,  9),
                new DateOnly(2025,  8, 13),
                new DateOnly(2025,  9, 10),
                new DateOnly(2025, 10,  8),
                new DateOnly(2025, 11, 12),
                new DateOnly(2025, 12, 10)
            };

            CollectionAssert.AreEqual(
                expected,
                dates,
                "2nd Wednesdays in the year do not match the expected dates.");

            // Extra safety checks: all are Wednesdays, and all are 2nd occurrence
            foreach (var d in dates)
            {
                Assert.AreEqual(DayOfWeek.Wednesday, d.DayOfWeek, $"{d} should be a Wednesday.");

                // Ensure it's the 2nd Wednesday of that month
                int count = 0;
                var cursor = new DateOnly(d.Year, d.Month, 1);
                while (cursor <= d)
                {
                    if (cursor.DayOfWeek == DayOfWeek.Wednesday)
                        count++;

                    cursor = cursor.AddDays(1);
                }

                Assert.AreEqual(2, count, $"{d} should be the 2nd Wednesday of its month.");
            }
        }

        [TestMethod]
        public void LastFridayOfEachMonth_ReturnsCorrectDates()
        {
            var rule = new DateTimeOfTheYearRule
            {
                DayOfWeekValue = DayOfWeek.Friday,
                LastWeekOfMonth = true
            };

            int year = 2025;

            var dates = rule.GetDates(year)
                            .OrderBy(d => d)
                            .ToList();

            // For debugging if the test fails:
            if (dates.Count != 12)
            {
                Console.WriteLine("Actual dates found:");
                foreach (var d in dates)
                {
                    Console.WriteLine($"{d} ({d.DayOfWeek})");
                }
            }

            // 1) Should be exactly 12 dates (one per month)
            Assert.AreEqual(12, dates.Count, "Should have one last Friday per month.");

            var expected = new[]
            {
                new DateOnly(2025,  1, 31),
                new DateOnly(2025,  2, 28),
                new DateOnly(2025,  3, 28),
                new DateOnly(2025,  4, 25),
                new DateOnly(2025,  5, 30),
                new DateOnly(2025,  6, 27),
                new DateOnly(2025,  7, 25),
                new DateOnly(2025,  8, 29),
                new DateOnly(2025,  9, 26),
                new DateOnly(2025, 10, 31),
                new DateOnly(2025, 11, 28),
                new DateOnly(2025, 12, 26)
            };

            CollectionAssert.AreEqual(
                expected,
                dates,
                "Last Fridays of each month in 2025 do not match expected values.");

            // Extra safety: each is actually a Friday and last one in its month
            foreach (var d in dates)
            {
                Assert.AreEqual(DayOfWeek.Friday, d.DayOfWeek, $"{d} should be Friday.");

                var cursor = d.AddDays(1);
                while (cursor.Month == d.Month)
                {
                    Assert.AreNotEqual(
                        DayOfWeek.Friday,
                        cursor.DayOfWeek,
                        $"{d} is not the last Friday; {cursor} is also a Friday.");
                    cursor = cursor.AddDays(1);
                }
            }
        }
    }
}
