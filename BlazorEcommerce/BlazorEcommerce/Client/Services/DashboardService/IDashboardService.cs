namespace BlazorEcommerce.Client.Services.DashboardService;

public interface IDashboardService
{
    event Action? OnChange;
    public DashboardDto PropertyCount { get; set; }
    Task MainDashboard();
}
