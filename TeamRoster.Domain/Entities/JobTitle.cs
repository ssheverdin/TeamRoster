using System;
using System.Collections.Generic;
using System.Text;
using TeamRoster.Domain.Base;

namespace TeamRoster.Domain.Entities
{
    public class JobTitle : EntityBase
    {
        public int TenantId { get; private set; }
        public Tenant Tenant { get; private set; } = default!;

        public string Name { get; private set; } = default!;

        private JobTitle() { }

        public JobTitle(Tenant tenant, string name)
        {
            Tenant = tenant ?? throw new ArgumentNullException(nameof(tenant));
            TenantId = tenant.Id;

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Job title name is required", nameof(name));

            Name = name.Trim();
        }

        public JobTitle Rename(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Job title name is required", nameof(name));

            Name = name.Trim();
            return this;
        }
    }
}
