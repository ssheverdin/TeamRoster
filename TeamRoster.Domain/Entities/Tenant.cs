using System;
using System.Collections.Generic;
using System.Text;
using TeamRoster.Domain.Base;

namespace TeamRoster.Domain.Entities;

public class Tenant : EntityBase
{
    public string Name { get; private set; } = default!;
    public string? Code { get; private set; } // short code, optional

    public DayOfWeek StartOfWeek { get; private set; } = DayOfWeek.Monday;
    public string TimeZoneId { get; private set; } = TimeZoneInfo.Local.Id;

    public ICollection<Employee> Employees { get; private set; } = new List<Employee>();
    public ICollection<Shift> Shifts { get; private set; } = new List<Shift>();

    // tenant-scoped job titles and sites
    public ICollection<JobTitle> JobTitles { get; private set; } = new List<JobTitle>();
    public ICollection<Site> Sites { get; private set; } = new List<Site>();
    public ICollection<Location> Locations { get; private set; } = new List<Location>();

    private Tenant() { }

    public Tenant(string name, string? code = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tenant name is required.", nameof(name));

        Name = name.Trim();
        Code = code?.Trim();
    }
}
