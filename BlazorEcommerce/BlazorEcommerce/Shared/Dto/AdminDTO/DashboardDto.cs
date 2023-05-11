using BlazorEcommerce.Shared.Models;

namespace BlazorEcommerce.Shared.Dto.AdminDTO;
public class DashboardDto
{
    public IEnumerable<Product> Products { get; set; } = new List<Product>();
    public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    public IEnumerable<User> Users { get; set; } = new List<User>();
    public IEnumerable<Order> Orders { get; set; } = new List<Order>();
    public IEnumerable<ProductType> ProductTypes { get; set; } = new List<ProductType>();
    public IncomeDto IncomeDto { get; set; }
}
