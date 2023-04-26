namespace BlazorEcommerce.Server.Services.Repositories.IRepositories;

public interface IImagesRepository : IRepository<Image>
{
	void Update(Image image);
}