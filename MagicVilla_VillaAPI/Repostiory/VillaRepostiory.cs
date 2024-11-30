using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repostiory.IRepostiory;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repostiory
{
	public class VillaRepostiory :Repository<Villa> , IVillaRepository
	{
		private readonly ApplicationDbContext _db;

		public VillaRepostiory(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

	

		public async Task<Villa> UpdateAsync(Villa Entity)
		{

			Entity.UpdateDate = DateTime.Now;
			_db.Villas.Update(Entity);

			await _db.SaveChangesAsync();
			return Entity;
		}
	}
}