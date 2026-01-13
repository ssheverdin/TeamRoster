using Microsoft.EntityFrameworkCore;
using TeamRoster.Domain.Entities;

namespace TeamRoster.DataAccess
{
    public class TeamRosterDbContext : DbContext
    {
        public TeamRosterDbContext(DbContextOptions<TeamRosterDbContext> options) : base(options)
        {
        }

        public DbSet<Person> People { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Tenant> Tenants { get; set; } = null!;
        public DbSet<Shift> Shifts { get; set; } = null!;
        public DbSet<ShiftAssignment> ShiftAssignments { get; set; } = null!;
        public DbSet<JobTitle> JobTitles { get; set; } = null!;
        public DbSet<Site> Sites { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<ShiftBlock> ShiftBlocks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // EntityBase common properties
            modelBuilder.Entity<Person>(eb =>
            {
                eb.HasKey(e => e.Id);
                eb.Property(e => e.FirstName).IsRequired();
                eb.Property(e => e.LastName).IsRequired();
                eb.Property(e => e.Email).IsRequired();
                eb.HasIndex(e => e.Email).IsUnique();

                eb.HasMany(e => e.Employees).WithOne(e => e.Person).HasForeignKey(e => e.PersonId);
                eb.HasOne(e => e.User).WithOne(u => u.Person).HasForeignKey<User>(u => u.PersonId);
            });

            modelBuilder.Entity<Tenant>(eb =>
            {
                eb.HasKey(t => t.Id);
                eb.Property(t => t.Name).IsRequired();
            });

            modelBuilder.Entity<Employee>(eb =>
            {
                eb.HasKey(e => e.Id);
                eb.HasOne(e => e.Tenant).WithMany(t => t.Employees).HasForeignKey(e => e.TenantId);
                eb.HasOne(e => e.Person).WithMany(p => p.Employees).HasForeignKey(e => e.PersonId);
                eb.HasOne(e => e.JobTitle).WithMany().HasForeignKey(e => e.JobTitleId);
            });

            modelBuilder.Entity<User>(eb =>
            {
                eb.HasKey(u => u.Id);
                eb.Property(u => u.UserName).IsRequired();
                eb.HasIndex(u => u.UserName).IsUnique();
            });

            modelBuilder.Entity<JobTitle>(eb =>
            {
                eb.HasKey(j => j.Id);
                eb.Property(j => j.Name).IsRequired();
                eb.HasOne(j => j.Tenant).WithMany(t => t.JobTitles).HasForeignKey(j => j.TenantId);
            });

            modelBuilder.Entity<Site>(eb =>
            {
                eb.HasKey(s => s.Id);
                eb.Property(s => s.Name).IsRequired();
                eb.HasOne(s => s.Tenant).WithMany(t => t.Sites).HasForeignKey(s => s.TenantId);
                eb.HasMany(s => s.Locations).WithOne(l => l.Site).HasForeignKey(l => l.SiteId).IsRequired(false);

                // Owned Address
                eb.OwnsOne(s => s.Address, a =>
                {
                    a.Property(p => p.Street).HasColumnName("Address_Street");
                    a.Property(p => p.City).HasColumnName("Address_City");
                    a.Property(p => p.State).HasColumnName("Address_State");
                    a.Property(p => p.PostalCode).HasColumnName("Address_PostalCode");
                    a.Property(p => p.Country).HasColumnName("Address_Country");
                });

                // Owned Coordinates separate
                eb.OwnsOne(s => s.Coordinates, c =>
                {
                    c.Property(x => x.Latitude).HasColumnName("Coordinates_Latitude");
                    c.Property(x => x.Longitude).HasColumnName("Coordinates_Longitude");
                });
            });

            modelBuilder.Entity<Location>(eb =>
            {
                eb.HasKey(l => l.Id);
                eb.Property(l => l.Name).IsRequired();
                eb.HasOne(l => l.Tenant).WithMany(t => t.Locations).HasForeignKey(l => l.TenantId);
                eb.HasOne(l => l.Site).WithMany(s => s.Locations).HasForeignKey(l => l.SiteId).IsRequired(false);
            });

            modelBuilder.Entity<Shift>(eb =>
            {
                eb.HasKey(s => s.Id);
                eb.Property(s => s.Name).IsRequired();
                eb.HasOne(s => s.Tenant).WithMany(t => t.Shifts).HasForeignKey(s => s.TenantId);
                eb.HasOne(s => s.Site).WithMany().HasForeignKey(s => s.SiteId).IsRequired(false);
                eb.HasOne(s => s.Location).WithMany().HasForeignKey(s => s.LocationId).IsRequired(false);
                eb.HasOne(s => s.JobTitle).WithMany().HasForeignKey(s => s.JobTitleId).IsRequired(false);
            });

            modelBuilder.Entity<ShiftAssignment>(eb =>
            {
                eb.HasKey(sa => sa.Id);
                eb.HasOne(sa => sa.Shift).WithMany(s => s.Assignments).HasForeignKey(sa => sa.ShiftId);
                eb.HasOne(sa => sa.Employee).WithMany(e => e.Assignments).HasForeignKey(sa => sa.EmployeeId);
            });

            modelBuilder.Entity<ShiftBlock>(eb =>
            {
                eb.HasKey(sb => sb.Id);
                eb.Property(sb => sb.Name).IsRequired();
                eb.HasOne(sb => sb.Tenant).WithMany().HasForeignKey(sb => sb.TenantId);
                eb.HasOne(sb => sb.Site).WithMany().HasForeignKey(sb => sb.SiteId).IsRequired(false);
                eb.HasOne(sb => sb.Location).WithMany().HasForeignKey(sb => sb.LocationId).IsRequired(false);
                eb.HasOne(sb => sb.JobTitle).WithMany().HasForeignKey(sb => sb.JobTitleId).IsRequired(false);
            });
        }
    }
}
