using BlazorEcommerce.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
	private readonly AppDbContext _db;

	public ProductController(AppDbContext db)
    {
		_db = db;
	}

	[HttpGet("Get-all-products")]
	public async Task<ActionResult<List<Product>>> GetAllProducts()
	{
		var products = await _db.Products.ToListAsync();
		return Ok(products);
	}
}
