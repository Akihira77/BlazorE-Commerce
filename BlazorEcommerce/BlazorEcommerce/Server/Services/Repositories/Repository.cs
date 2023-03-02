using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using System.Linq.Expressions;

namespace BlazorEcommerce.Server.Services.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
	private readonly AppDbContext _db;
	internal DbSet<T> dbSet;
	public Repository(AppDbContext db)
    {
		_db = db;
		this.dbSet = db.Set<T>();

	}
    public async Task Add(T entity)
	{
		await dbSet.AddAsync(entity);
	}

	public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null)
	{
		IQueryable<T> query = dbSet;
		
		if (filter != null)
		{
			query = query.Where(filter);
		}

		return await query.ToListAsync();
	}

	public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter, bool track = true)
	{
		IQueryable<T> query = (track ? dbSet : dbSet.AsNoTracking());

		return await query.FirstOrDefaultAsync(filter);
	}

	public void Remove(T entity)
	{
		dbSet.Remove(entity);
	}

	public void RemoveRange(IEnumerable<T> entities)
	{
		dbSet.RemoveRange(entities);
	}
}
