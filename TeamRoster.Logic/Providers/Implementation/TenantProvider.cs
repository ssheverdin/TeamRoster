using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamRoster.DataAccess;
using TeamRoster.Domain.Entities;
using TeamRoster.Dtos;
using TeamRoster.Logic.Mappings;

namespace TeamRoster.Logic.Providers.Implementation
{
    public class TenantProvider : ITenantProvider
    {
        private IUnitOfWork _unitOfWork;

        public TenantProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
        public async Task<List<TenantDto>> GetAllTenants()
        {
            List<Tenant> tenants = await _unitOfWork.Databse.Tenants.ToListAsync();

            return tenants.Select(tenant => TenantMapper.MapToDto(tenant)).ToList();
        }

        public async Task<TenantDto> GetTenantByIdAsync(int tenantId)
        {
            Tenant? tenant = await _unitOfWork.Databse.Tenants.FirstOrDefaultAsync(t => t.Id == tenantId);
            if (tenant != null)
            {
                return TenantMapper.MapToDto(tenant);
            }
            return new TenantDto();
        }
    }
}
