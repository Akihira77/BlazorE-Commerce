﻿using BlazorEcommerce.Shared.Models;

namespace BlazorEcommerce.Client.Services.CategoryService;

public interface ICategoryRepository
{
	IEnumerable<Category> Categories { get; set; }
	Task GetCategories();
	Task<ServiceResponse<Category>> GetCategory(int id);
}
