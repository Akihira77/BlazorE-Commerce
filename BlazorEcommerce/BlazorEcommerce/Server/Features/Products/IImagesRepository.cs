using BlazorEcommerce.Server.Features.Base;

namespace BlazorEcommerce.Server.Features.Products;

public interface IImagesRepository : IRepository<Image>
{
	void Update(Image image);
}
