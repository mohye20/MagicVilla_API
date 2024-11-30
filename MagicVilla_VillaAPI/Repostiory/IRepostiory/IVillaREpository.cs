using MagicT_TAPI.Repostiory.IRepostiory;
using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repostiory.IRepostiory
{
	public interface IVillaRepository : IRepository<Villa>
	{
		

		Task<Villa> UpdateAsync(Villa Entity);

		
	}
}