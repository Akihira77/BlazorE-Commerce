using System.Linq.Expressions;

namespace BlazorEcommerce.Server.Services.Repositories.IRepositories;

public interface IRepository<T> where T : class
{
	Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool track = true);
	Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool track = true);
	Task Add(T entity);
	Task AddRange(IEnumerable<T> entities);
	void Remove(T entity);
	void RemoveRange(IEnumerable<T> entities);
}
