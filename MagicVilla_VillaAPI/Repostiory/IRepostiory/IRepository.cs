using System.Linq.Expressions;

namespace MagicT_TAPI.Repostiory.IRepostiory
{
	public interface IRepository<T> where T :class
	{


		Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? Filter = null);

		Task<T> GetAsync(Expression<Func<T, bool>> Filter = null, bool Tracked = true);

		Task CreateAsync(T Entity);

		Task RemoveAsync(T Entity);


		Task SaveAsync();
	}
}

