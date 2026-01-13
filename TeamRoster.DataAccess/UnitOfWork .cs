using System;
using System.Collections.Generic;
using System.Text;

namespace TeamRoster.DataAccess
{
    public sealed class EfUnitOfWork : IUnitOfWork
    {
        private readonly TeamRosterDbContext _db;

        public EfUnitOfWork(TeamRosterDbContext db) => _db = db;

        //It has repositoeris already!
        public TeamRosterDbContext Database => _db;

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => _db.SaveChangesAsync(cancellationToken);
    }

    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        public TeamRosterDbContext Databse {  get; }
    }
}
