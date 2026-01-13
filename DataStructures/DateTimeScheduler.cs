using DataStructures.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures;


public enum RecurrenceFrequency
{
    None,
    Daily,
    Weekly,
    BiWeekly,
    Monthly,
    Yearly
}

public class DateTimeScheduler
{
    /// <summary>
    /// Structural date rules (month, day-of-week, nth weekday, etc.).
    /// </summary>
    public List<DateTimeOfTheYearRule> DateTimeOfTheYearRules { get; set; } = new();

    public RecurrenceFrequency RecurrenceFrequency { get; set; } = RecurrenceFrequency.None;

    public DayOfWeek FirstDayOfTheWeek { get; set; } = DayOfWeek.Monday;

    public bool AdjustToFullWeeks { get; set; }

    public List<DateTimeRange> GetDateTimeRanges( DateTimeRange range)
    {
        var result = new List<DateTimeRange>();

        // Optional: explicit behavior when there are no rules
        if (!DateTimeOfTheYearRules.Any() &&
            RecurrenceFrequency != RecurrenceFrequency.None)
        {
            // No rules + recurrence → no matches (or change as you like)
            return result;
        }

        switch (RecurrenceFrequency)
        {
            case RecurrenceFrequency.None:
                AddFirstMatch(range, result);
                break;

            case RecurrenceFrequency.Daily:
                AddDailyLikeMatches(range, result);
                break;

            case RecurrenceFrequency.Weekly:
                AddWeeklyMatches(range, result);
                break;

            case RecurrenceFrequency.BiWeekly:
                if(DateTimeOfTheYearRules.Any(i => i.FirstWeekRule || i.SecondWeekRule))
                {
                    AddBiWeeklyForWeeksMatches(range, result);
                }
                else
                    AddBiWeeklyForWeekMatches(range, result);
                break;

            case RecurrenceFrequency.Monthly:
            case RecurrenceFrequency.Yearly:
                // For now, reuse daily behavior; structural rules define the pattern.
                AddDailyLikeMatches(range, result);
                break;
        }

        return result;
    }

    private void AddFirstMatch(DateTimeRange range, List<DateTimeRange> result)
    {
        var allDates = range.GetDates();
        foreach (var date in allDates)
        {
            foreach (var rule in DateTimeOfTheYearRules)
            {
                if (!rule.Match(date))
                    continue;

                result.Add(rule.ToDateTimeRange(date));
                return; // only the first matching rule/date
            }
        }
    }

    private void AddDailyLikeMatches(DateTimeRange range, List<DateTimeRange> result)
    {
        var allDates = range.GetDates();
        foreach (var date in allDates)
        {
            foreach (var rule in DateTimeOfTheYearRules)
            {
                if (!rule.Match(date))
                    continue;

                result.Add(rule.ToDateTimeRange(date));
            }
        }
    }

    private void AddWeeklyMatches(DateTimeRange range, List<DateTimeRange> result)
    {
        var datesByWeek = range.GetDatesByWeek(FirstDayOfTheWeek, adjustToWeekRange: AdjustToFullWeeks);

        var weekRulse = DateTimeOfTheYearRules
            .ToList();

        foreach (var week in datesByWeek)
        {
            foreach (var date in week.Value)
            {
                foreach (var rule in weekRulse)
                {
                    if (!rule.Match(date))
                        continue;

                    result.Add(rule.ToDateTimeRange(date));
                }
            }
        }
    }

    private void AddBiWeeklyForWeeksMatches(DateTimeRange range, List<DateTimeRange> result)
    {
        var datesByWeek = range.GetDatesByWeek(FirstDayOfTheWeek, adjustToWeekRange: AdjustToFullWeeks);

        var firstWeekRules = DateTimeOfTheYearRules
            .Where(r => r.FirstWeekRule)
            .ToList();

        var secondWeekRules = DateTimeOfTheYearRules
            .Where(r => r.SecondWeekRule)
            .ToList();

        bool useFirstWeekRules = true;

        foreach (var week in datesByWeek)
        {
            var weekRules = useFirstWeekRules ? firstWeekRules : secondWeekRules;

            foreach (var date in week.Value)
            {
                foreach (var rule in weekRules)
                {
                    if (!rule.Match(date))
                        continue;

                    result.Add(rule.ToDateTimeRange(date));
                }
            }

            useFirstWeekRules = !useFirstWeekRules;
        }
    }

    private void AddBiWeeklyForWeekMatches(DateTimeRange range, List<DateTimeRange> result)
    {
        var datesByWeek = range.GetDatesByWeek(FirstDayOfTheWeek, adjustToWeekRange: AdjustToFullWeeks);

        var weekRulse = DateTimeOfTheYearRules
            .ToList();

        bool useWeekRules = true;

        foreach (var week in datesByWeek)
        {
            

            foreach (var date in week.Value)
            {
                foreach (var rule in weekRulse)
                {
                    if(!useWeekRules)
                        continue;

                    if (!rule.Match(date))
                        continue;

                    result.Add(rule.ToDateTimeRange(date));
                }
            }

            useWeekRules = !useWeekRules;
        }
    }
}