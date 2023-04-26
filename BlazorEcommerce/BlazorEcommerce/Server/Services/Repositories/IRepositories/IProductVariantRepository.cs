namespace BlazorEcommerce.Server.Services.Repositories.IRepositories;

public interface IProductVariantRepository : IRepository<ProductVariant>
{
	void Update(ProductVariant productVariant);
}