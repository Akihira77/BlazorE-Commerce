namespace BlazorEcommerce.Server.Services.Repositories.IRepositories;

public interface IProductTypeRepository : IRepository<ProductType>
{
    void Update(ProductType productType);
}
