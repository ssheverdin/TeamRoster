using System;
using System.Collections.Generic;
using System.Text;
using TeamRoster.Domain.Base;

namespace TeamRoster.Domain.Entities
{
    public class ShiftAssignment : EntityBase
    {
        public Guid EmployeeId { get; private set; }
        public Guid ShiftId { get; private set; }

        public Employee Employee { get; private set; } = default!;
        public Shift Shift { get; private set; } = default!;

        private ShiftAssignment() { }

        public ShiftAssignment(Guid employeeId, Guid shiftId)
        {
            EmployeeId = employeeId;
            ShiftId = shiftId;
        }
    }
}
