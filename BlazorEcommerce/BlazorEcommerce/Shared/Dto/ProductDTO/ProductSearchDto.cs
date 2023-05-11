using BlazorEcommerce.Shared.Models;

namespace BlazorEcommerce.Shared.Dto.ProductDTO;
public class ProductSearchDto
{
    public IEnumerable<Product> Products { get; set; }
    public int Pages { get; set; }
    public int CurrentPage { get; set; }
}
