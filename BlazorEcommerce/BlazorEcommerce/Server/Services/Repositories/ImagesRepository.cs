using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;

namespace BlazorEcommerce.Server.Services.Repositories;

public class ImagesRepository : Repository<Image>, IImagesRepository
{
	private readonly AppDbContext _db;

	public ImagesRepository(AppDbContext db) : base(db)
	{
		_db = db;
	}

	public void Update(Image image)
	{
		_db.Images.Update(image);
	}
}
