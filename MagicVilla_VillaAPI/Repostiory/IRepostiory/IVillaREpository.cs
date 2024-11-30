using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repostiory.IRepostiory
{
	public interface IVillaREpository
	{
		Task<List<Villa>> GetAll(Expression<Func<Villa>> Filter = null);

		Task<Villa> Get(Expression<Func<Villa>> Filter = null);

		Task Create(Villa Entity);

		Task Remove(Villa Entity);

		Task Save();
	}
}