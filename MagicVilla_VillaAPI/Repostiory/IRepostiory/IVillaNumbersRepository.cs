using MagicT_TAPI.Repostiory.IRepostiory;
using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repostiory.IRepostiory
{
	public interface IVillaNumbersRepository : IRepository<VillaNumber>
	{

		Task<VillaNumber> UpdateAsync(VillaNumber entity);
	}
}
