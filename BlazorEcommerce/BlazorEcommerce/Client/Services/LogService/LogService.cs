namespace BlazorEcommerce.Client.Services.LogService;

public class LogService : ILogService
{
	private readonly HttpClient _http;
	public List<Logs> Logs { get; set; } = new List<Logs>();

	public event Action? LogsChanged;

	public LogService(HttpClient http)
    {
		_http = http;
	}


	public async Task GetAllLogs()
	{
		var result = await _http.GetFromJsonAsync<ServiceResponse<IEnumerable<Logs>>>("api/v1/logs");

		Logs = result.Data.ToList();
		LogsChanged?.Invoke();
	}
}
