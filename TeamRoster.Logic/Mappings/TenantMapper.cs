using System;
using System.Collections.Generic;
using System.Text;
using TeamRoster.Domain.Entities;
using TeamRoster.Dtos;

namespace TeamRoster.Logic.Mappings
{
    public static class TenantMapper
    {
        public static TenantDto MapToDto(Tenant tenant)
        {
            return new TenantDto()
            {
                Name = tenant.Name,
                Code = tenant.Code,
            };
        }
    }
}
