using System;
using System.Collections.Generic;
using System.Text;

namespace TeamRoster.Dtos
{
    public class EmployeeDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public int MaxHoursPerWeek { get; set; }
    }
}
