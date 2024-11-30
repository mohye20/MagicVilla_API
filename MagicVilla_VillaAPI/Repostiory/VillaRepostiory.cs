using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repostiory.IRepostiory;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repostiory
{
	public class VillaRepostiory : IVillaRepository
	{
		private readonly ApplicationDbContext _db;

		public VillaRepostiory(ApplicationDbContext db)
		{
			_db = db;
		}

		public async Task Create(Villa Entity)
		{
			await _db.Villas.AddAsync(Entity);
			await Save();
		}

		public async Task<Villa> Get(Expression<Func<Villa, bool>> Filter = null, bool Tracked = true)
		{
			IQueryable<Villa> Query = _db.Villas;
			if(!Tracked)
			{ 
				Query = Query.AsNoTracking();	
			}

			if(Filter is not null)
			{
				Query = Query.Where(Filter);
			}

			return await Query.FirstOrDefaultAsync();
		}

		public async Task<List<Villa>> GetAll(Expression<Func<Villa, bool>> Filter = null)
		{
				IQueryable<Villa> query = _db.Villas;
			if (Filter is not null)
			{
				query = query.Where(Filter);	

			}

			return await query.ToListAsync();
		}

		public async Task Remove(Villa Entity)
		{
			_db.Villas.Remove(Entity);
			await Save();
		}

		public async Task Save()
		{
			await _db.SaveChangesAsync();
		}
	}
}