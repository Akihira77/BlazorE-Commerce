global using BlazorEcommerce.Client.Services.OrderService;
global using BlazorEcommerce.Client.Services.AuthService;
global using BlazorEcommerce.Client.Services.CartService;
global using BlazorEcommerce.Client.Services.CategoryService;
global using BlazorEcommerce.Client.Services.ProductService;
global using BlazorEcommerce.Client.Services.AddressService;
global using BlazorEcommerce.Client.Services.DashboardService;
global using BlazorEcommerce.Client.Services.LogService;
global using BlazorEcommerce.Shared;
global using BlazorEcommerce.Shared.Models;
global using Microsoft.AspNetCore.Components.Authorization;
global using System.Net.Http.Json;
global using BlazorEcommerce.Client.Services.ProductTypeService;

using BlazorEcommerce.Client;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();	
builder.Services.AddScoped<IAddressService, AddressService>();	
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();	
builder.Services.AddScoped<IDashboardService, DashboardService>();	
builder.Services.AddScoped<ILogService, LogService>();	
builder.Services.AddScoped<LazyAssemblyLoader>();

builder.Services.AddMudServices(config =>
{
	config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;

	config.SnackbarConfiguration.PreventDuplicates = true;
	config.SnackbarConfiguration.NewestOnTop = true;
	config.SnackbarConfiguration.ShowCloseIcon = true;
	config.SnackbarConfiguration.VisibleStateDuration = 3000;
	config.SnackbarConfiguration.HideTransitionDuration = 500;
	config.SnackbarConfiguration.ShowTransitionDuration = 500;
	config.SnackbarConfiguration.SnackbarVariant = Variant.Outlined;
});

// authorization
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

await builder.Build().RunAsync();
