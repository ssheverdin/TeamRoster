using DataStructures;
using System;
using System.Collections.Generic;
using System.Text;
using TeamRoster.Domain.Base;

namespace TeamRoster.Domain.Entities
{
    public class Shift : EntityBase
    {
        public int TenantId { get; private set; }
        public Tenant Tenant { get; private set; } = default!;

        public string Name { get; private set; } = default!;
        public DateTime StartUtc { get; private set; }
        public DateTime EndUtc { get; private set; }

        public DateTime? IsPublished { get; private set; }

        // Geographical site reference (tenant-scoped)
        public int? SiteId { get; private set; }
        public Site? Site { get; private set; }

        // Physical location reference within the site
        public int? LocationId { get; private set; }
        public Location? Location { get; private set; }

        // Optional job title required for this shift
        public int? JobTitleId { get; private set; }
        public JobTitle? JobTitle { get; private set; }

        public ColorHex Color { get; private set; }

        public int RequestedByEmployeeId { get; private set; }
        public DateTime? RequestedAtUtc { get; private set; }
        public int PostedByEmployeeId { get; private set; }
        public DateTime? PostedAtUtc { get; private set; }


        public ICollection<ShiftAssignment> Assignments { get; private set; } = new List<ShiftAssignment>();

        private Shift() { }

        public Shift(
            Tenant tenant,
            string name,
            DateTime startUtc,
            DateTime endUtc,
            Site? site,
            Location? location,
            JobTitle? jobTitle,
            ColorHex color)
        {
            Tenant = tenant ?? throw new ArgumentNullException(nameof(tenant));
            TenantId = tenant.Id;

            if (endUtc <= startUtc)
                throw new ArgumentException("Shift end must be after start.");

            Name = name;
            StartUtc = startUtc;
            EndUtc = endUtc;
            Site = site;
            SiteId = site?.Id;
            Location = location;
            LocationId = location?.Id;

            if (jobTitle != null && jobTitle.TenantId != TenantId)
                throw new ArgumentException("JobTitle tenant mismatch", nameof(jobTitle));

            JobTitle = jobTitle;
            JobTitleId = jobTitle?.Id;

            Color = color;
        }

        public Shift UpdateLocation(Location? location)
        {
            Location = location;
            LocationId = location?.Id;
            // ensure site sync if location provided
            if (location != null)
            {
                Site = location.Site;
                SiteId = location.SiteId;
            }
            return this;
        }

        public Shift SetJobTitle(JobTitle? jobTitle)
        {
            if (jobTitle != null && jobTitle.TenantId != TenantId)
                throw new ArgumentException("JobTitle tenant mismatch", nameof(jobTitle));

            JobTitle = jobTitle;
            JobTitleId = jobTitle?.Id;
            return this;
        }
    }
}
