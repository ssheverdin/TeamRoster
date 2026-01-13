using System;
using System.Collections.Generic;
using System.Text;
using TeamRoster.Domain.Base;

namespace TeamRoster.Domain.Entities
{
    public class Employee : EntityBase
    {
        public int PersonId { get; private set; }
        public Person Person { get; private set; } = default!;

        public int TenantId { get; private set; }
        public Tenant Tenant { get; private set; } = default!;

        public bool IsActive { get; private set; } = true;
        public int MaxHoursPerWeek { get; private set; } = 40;

        // JobTitle is now a tenant-scoped entity
        public int? JobTitleId { get; private set; }
        public JobTitle? JobTitle { get; private set; }

        public ICollection<ShiftAssignment> Assignments { get; private set; } = new List<ShiftAssignment>();

        private Employee() { }

        public Employee(Person person, Tenant tenant, int maxHoursPerWeek = 40, JobTitle? jobTitle = null)
        {
            Person = person ?? throw new ArgumentNullException(nameof(person));
            Tenant = tenant ?? throw new ArgumentNullException(nameof(tenant));

            PersonId = person.Id;
            TenantId = tenant.Id;

            SetMaxHoursPerWeek(maxHoursPerWeek);
            JobTitle = jobTitle;
            JobTitleId = jobTitle?.Id;
        }

        public Employee SetMaxHoursPerWeek(int hours)
        {
            if (hours <= 0)
                throw new ArgumentException("Max hours per week must be positive.", nameof(hours));

            MaxHoursPerWeek = hours;
            return this;
        }

        public Employee Deactivate() { IsActive = false; return this; }
        public Employee Activate() { IsActive = true; return this; }

        public Employee SetJobTitle(JobTitle? jobTitle)
        {
            JobTitle = jobTitle;
            JobTitleId = jobTitle?.Id;
            return this;
        }
    }
}
