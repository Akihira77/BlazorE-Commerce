namespace BlazorEcommerce.Server.Services.Repositories.IRepositories;

public interface IUnitOfWork
{
	IProductRepository Product { get; }
	ICategoryRepository Category { get; }
	ICartRepository Cart { get; }
	IAuthRepository Auth { get; }
	IOrderRepository Order { get; }
	IPaymentRepository Payment { get; }
	IAddressRepository Address { get; }
	IProductTypeRepository ProductType { get; }
	IProductVariantRepository ProductVariant { get; }
	IImagesRepository Images { get; }
	IOrderHeaderRepository OrderHeader { get; }
	ISendOrderRepository SendOrder { get; }
	IProductRatingsRepository ProductRatings { get; }
	ILogRepository Log { get; }

	Task SaveAsync();

	void Save();
}