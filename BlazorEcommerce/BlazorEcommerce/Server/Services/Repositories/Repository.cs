using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using System.Linq.Expressions;
using System.Security.Claims;

namespace BlazorEcommerce.Server.Services.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
	//private readonly AppDbContext _db;
	internal DbSet<T> _dbSet;

	public Repository(AppDbContext db)
	{
		//_db = db;
		_dbSet = db.Set<T>();
	}
	public async Task Add(T entity)
	{
		await _dbSet.AddAsync(entity);
	}

	public async Task AddRange(IEnumerable<T> entities)
	{
		await _dbSet.AddRangeAsync(entities);
	}

	public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
	{
		IQueryable<T> query = _dbSet;

		if(filter != null)
		{
			query = query.Where(filter);
		}

		if(includeProperties != null)
		{
			foreach(var property in includeProperties.Split(",", StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(property);
			}
		}
		return await query.ToListAsync();
	}

	public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool track = true)
	{
		IQueryable<T> query = (track ? _dbSet : _dbSet.AsNoTracking());

		if(includeProperties != null)
		{
			foreach(var property in includeProperties.Split(",", StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(property);
			}
		}
		return await query.FirstOrDefaultAsync(filter);
	}

	public void Remove(T entity)
	{
		_dbSet.Remove(entity);
	}

	public void RemoveRange(IEnumerable<T> entities)
	{
		_dbSet.RemoveRange(entities);
	}
}
