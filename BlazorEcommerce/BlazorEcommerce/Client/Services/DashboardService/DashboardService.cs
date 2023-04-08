namespace BlazorEcommerce.Client.Services.DashboardService;

public class DashboardService : IDashboardService
{
    private readonly HttpClient _http;

    public DashboardDto PropertyCount { get; set; }

    public event Action? OnChange;

    public DashboardService(HttpClient http)
    {
        _http = http;
    }


    public async Task MainDashboard()
    {
        var result = await _http.GetFromJsonAsync<ServiceResponse<DashboardDto>>("api/v1/Dashboard");
        PropertyCount = result.Data;

        OnChange?.Invoke();
    }
}
