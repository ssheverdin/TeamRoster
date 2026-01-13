using System;
using System.Collections.Generic;
using System.Text;

namespace TeamRoster.Domain.Base
{
    public abstract class EntityBase
    {
        public int Id { get; set; }
        public Guid Oid { get; set; } = Guid.NewGuid();
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedUtc { get; set; }
        public DateTime? DeletedUtc { get; set; }
    }
}
