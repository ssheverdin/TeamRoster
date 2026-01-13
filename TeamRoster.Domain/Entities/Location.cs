using System;
using System.Collections.Generic;
using System.Text;
using TeamRoster.Domain.Base;

namespace TeamRoster.Domain.Entities
{
    public class Location : EntityBase
    {
        public int TenantId { get; private set; }
        public Tenant Tenant { get; private set; } = default!;

        public int? SiteId { get; private set; }
        public Site? Site { get; private set; }

        // physical location name within the site (e.g. "3rd floor", "Room 12")
        public string Name { get; private set; } = default!;
        public string? Description { get; private set; }

        private Location() { }

        // Primary ctor: create a Location for a Tenant, optional Site
        public Location(Tenant tenant, string name, Site? site = null, string? description = null)
        {
            Tenant = tenant ?? throw new ArgumentNullException(nameof(tenant));
            TenantId = tenant.Id;

            if (site != null)
            {
                Site = site;
                SiteId = site.Id;
            }

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Location name is required", nameof(name));

            Name = name.Trim();
            Description = description?.Trim();
        }

        // Convenience ctor kept for backward compatibility
        public Location(Site site, string name, string? description = null)
            : this(site?.Tenant ?? throw new ArgumentNullException(nameof(site)), name, site, description)
        {
        }

        public Location Rename(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Location name is required", nameof(name));

            Name = name.Trim();
            return this;
        }

        public Location UpdateDescription(string? description)
        {
            Description = description?.Trim();
            return this;
        }
    }
}
