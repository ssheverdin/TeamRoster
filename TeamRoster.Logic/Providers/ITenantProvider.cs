using System;
using System.Collections.Generic;
using System.Text;
using TeamRoster.Dtos;

namespace TeamRoster.Logic.Providers
{
    public interface ITenantProvider
    {
        List<TenantDto> GetAllTenants();
        TenantDto GetTenantByIdAsync(int tenantId);
    }
}
