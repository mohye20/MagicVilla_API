using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repostiory.IRepostiory
{
	public interface IVillaRepository
	{
		Task<List<Villa>> GetAll(Expression<Func<Villa , bool>> Filter = null);

		Task<Villa> Get(Expression<Func<Villa,bool>> Filter = null , bool Tracked = true);

		Task Create(Villa Entity);

		Task Remove(Villa Entity);

		Task Save();
	}
}