using MagicT_TAPI.Repostiory.IRepostiory;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repostiory
{
    public class Repository<T> : IRepository<T> where T : class
    {
        internal DbSet<T> dbSet;

        private readonly ApplicationDbContext _db;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            // _db.VillaNumbers.Include(U => U.Villa).ToList();
            this.dbSet = _db.Set<T>();
        }

        public async Task CreateAsync(T Entity)
        {
            await dbSet.AddAsync(Entity);
            await SaveAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> Filter = null, bool Tracked = true,
            string? includeProperties = null)
        {
            IQueryable<T> Query = dbSet;
            if (!Tracked)
            {
                Query = Query.AsNoTracking();
            }

            if (Filter is not null)
            {
                Query = Query.Where(Filter);
            }

            if (includeProperties is not null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' },
                             StringSplitOptions.RemoveEmptyEntries))
                {
                    Query = Query.Include(includeProp);
                }
            }

            return await Query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? Filter = null,
            string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (Filter is not null)
            {
                query = query.Where(Filter);
            }

            if (includeProperties is not null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' },
                             StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }

                ;
            }

            return await query.ToListAsync();
        }

        public async Task RemoveAsync(T Entity)
        {
            dbSet.Remove(Entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}