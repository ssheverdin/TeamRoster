using System;
using System.Collections.Generic;
using System.Text;

namespace TeamRoster.Dtos
{
    public class EmployeeListDto
    {
        public List<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
    }

    
}
