using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Features.Base;
using BlazorEcommerce.Shared.Dto.ProductDTO;
using Microsoft.EntityFrameworkCore.Query;

namespace BlazorEcommerce.Server.Features.Products;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly AppDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProductRepository(AppDbContext db, IHttpContextAccessor httpContextAccessor) : base(db)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Product> GetProduct(int id)
    {
        Product product = null;
        if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
        {
            product = await _db.Products
                .Include(p => p.Images)
                .Include(p => p.Variants
                        .Where(v => v.Visible && !v.Deleted))
                .ThenInclude(p => p.ProductType)
                .FirstOrDefaultAsync(p => p.Id == id && !p.Deleted);
        }
        else
        {
            product = await _db.Products
                .Include(p => p.Images)
                .Include(p => p.Ratings)
                .Include(p => p.Variants
                        .Where(v => v.Visible && !v.Deleted))
                .ThenInclude(p => p.ProductType)
                .FirstOrDefaultAsync(p => p.Id == id && !p.Deleted && p.Visible);
        }
        return product;
    }

    public async Task<IEnumerable<Product>> GetProductsByCategory(string? categoryUrl = null)
    {
        return await _db.Products
                .Where(p => p.Category.Url == categoryUrl
                        && p.Visible && !p.Deleted)
                .Include(p => p.Images)
                .Include(p => p.Ratings)
                .Include(p => p.Variants
                    .Where(v => v.Visible && !v.Deleted))
                    .ThenInclude(p => p.ProductType)
                .AsSplitQuery()
                .ToListAsync();
    }

    public List<string> GetProductSearchSuggestion(string searchText)
    {
        var obj = FindProductsBySearch(searchText);

        List<string> result = new();

        foreach (var item in obj)
        {
            if (item.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            {
                result.Add(item.Title);
            }

            if (item.Description != null)
            {
                var punctuation = item.Description
                    .Where(char.IsPunctuation)
                    .Distinct()
                    .ToArray();
                var words = item.Description
                    .Split()
                    .Select(s => s.Trim(punctuation));

                foreach (var word in words)
                {
                    if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                        && !result.Contains(word))
                    {
                        result.Add(word);
                    }
                }
            }
        }

        return result;
    }

    public async Task<ProductSearchDto> SearchProducts(string searchText, int page)
    {
        var pageResults = 5f;
        var pageCount = Math.Ceiling(FindProductsBySearch(searchText).Count() / pageResults);

        var obj = await _db.Products
            .Where(p => (p.Title
                        .ToLower()
                        .Contains(searchText.ToLower())
            || p.Description
                .ToLower()
                .Contains(searchText.ToLower()))
            && p.Visible && !p.Deleted)
            .Include(p => p.Images)
            .Include(p => p.Ratings)
            .Include(p => p.Variants
                    .Where(v => v.Visible && !v.Deleted))
            .Skip((page - 1) * (int)pageResults)
            .Take((int)pageResults)
            .ToListAsync();

        var data = new ProductSearchDto()
        {
            Products = obj,
            CurrentPage = page,
            Pages = (int)pageCount
        };
        return data;
    }

    private IIncludableQueryable<Product, IEnumerable<ProductVariant>> FindProductsBySearch(string searchText)
    {
        return _db.Products
            .Where(p => (p.Title
                        .ToLower()
                        .Contains(searchText.ToLower())
            || p.Description
                .ToLower()
                .Contains(searchText.ToLower()))
            && p.Visible && !p.Deleted)
            .Include(p => p.Variants
                    .Where(v => v.Visible && !v.Deleted));
    }

    public void Update(Product obj)
    {
        _db.Products.Update(obj);
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        var products = await _db.Products
                    .Where(p => p.Visible && !p.Deleted)
                    .Include(p => p.Images)
                    .Include(p => p.Ratings)
                    .Include(p => p.Variants
                        .Where(v => v.Visible && !v.Deleted))
                    .AsSplitQuery()
                    .ToListAsync();

        return products;
    }

    public async Task<IEnumerable<Product>> GetFeaturedProducts()
    {
        var products = await _db.Products
            .Where(p => p.Featured && p.Visible && !p.Deleted)
            .Include(p => p.Images)
            .Include(p => p.Ratings)
            .Include(p => p.Variants
                .Where(v => v.Visible && !v.Deleted))
            .AsSplitQuery()
            .ToListAsync();

        return products;
    }

    public async Task<IEnumerable<Product>> GetAdminProducts()
    {
        var products = await _db.Products
            .Include(p => p.Images)
            .Include(p => p.Category)
            .Include(p => p.Variants)
                .ThenInclude(v => v.ProductType)
            .AsSplitQuery()
            .ToListAsync();

        return products;
    }

    public async Task<IEnumerable<ProductRatings>> GetProductRatings(int id)
    {
        var productRatings = await _db.ProductRatings
                        .Where(pr => pr.ProductId == id)
                        .Include(pr => pr.User)
                        .ToListAsync();

        return productRatings;
    }
}