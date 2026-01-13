using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
using TeamRoster.Domain.Base;

namespace TeamRoster.Domain.Entities
{
    // ShiftBlock represents a template/time-block that can generate multiple shifts
    public class ShiftBlock : EntityBase
    {
        public int TenantId { get; private set; }
        public Tenant Tenant { get; private set; } = default!;

        public string Name { get; private set; } = default!;


        public TimeOnly? ShiftStart { get; set; }
        public TimeOnly? ShiftEnd { get; set; }
        

        public int? SiteId { get; private set; }
        public Site? Site { get; private set; }

        public int? LocationId { get; private set; }
        public Location? Location { get; private set; }

        public int? JobTitleId { get; private set; }
        public JobTitle? JobTitle { get; private set; }
    }
}
