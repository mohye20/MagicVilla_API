using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repostiory.IRepostiory
{
	public interface IVillaRepository
	{
		Task<List<Villa>> GetAllAsync(Expression<Func<Villa , bool>> Filter = null);

		Task<Villa> GetAsync(Expression<Func<Villa,bool>> Filter = null , bool Tracked = true);

		Task CreateAsync(Villa Entity);

		Task RemoveAsync(Villa Entity);

		Task UpdateAsync(Villa Entity);

		Task SaveAsync();
	}
}