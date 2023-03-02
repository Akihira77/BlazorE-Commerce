﻿using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;

namespace BlazorEcommerce.Server.Services.Repositories;

public class UnitOfWork : IUnitOfWork
{
	private readonly AppDbContext _db;
    public IProductRepository Product { get; private set; }

    public UnitOfWork(AppDbContext db)
    {
		_db = db;
		Product = new ProductRepository(_db);

	}
    public async Task Save()
	{
		await _db.SaveChangesAsync();
	}
}
