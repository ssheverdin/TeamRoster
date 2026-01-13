using System;
using System.Collections.Generic;
using System.Text;
using TeamRoster.Domain.Base;

namespace TeamRoster.Domain.Entities
{
    public class Site : EntityBase
    {
        public int TenantId { get; private set; }
        public Tenant Tenant { get; private set; } = default!;

        public string Name { get; private set; } = default!;
        // optional description for site (e.g. city, coordinates)
        public string? Description { get; private set; }

        public Address? Address { get; private set; }
        public Coordinates? Coordinates { get; private set; }

        public ICollection<Location> Locations { get; private set; } = new List<Location>();

        private Site() { }

        public Site(Tenant tenant, string name, string? description = null, Address? address = null, Coordinates? coordinates = null)
        {
            Tenant = tenant ?? throw new ArgumentNullException(nameof(tenant));
            TenantId = tenant.Id;

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Site name is required", nameof(name));

            Name = name.Trim();
            Description = description?.Trim();
            Address = address;
            Coordinates = coordinates;
        }

        public Site UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Site name is required", nameof(name));

            Name = name.Trim();
            return this;
        }

        public Site UpdateDescription(string? description)
        {
            Description = description?.Trim();
            return this;
        }

        public Site UpdateAddress(Address? address)
        {
            Address = address;
            return this;
        }

        public Site UpdateCoordinates(Coordinates? coordinates)
        {
            Coordinates = coordinates;
            return this;
        }
    }
}
