using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repostiory.IRepostiory;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Repostiory
{
	public class VillaNumbersRepository : Repository<VillaNumber>, IVillaNumbersRepository
	{
		private readonly ApplicationDbContext _db;

		public VillaNumbersRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		

		public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
		{
			entity.UpdateDate = DateTime.Now;

			_db.VillaNumbers.Update(entity);

			await _db.SaveChangesAsync();

			return entity;
		}
	}
}