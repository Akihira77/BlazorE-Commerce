namespace BlazorEcommerce.Client.Services.LogService;

public interface ILogService
{
	event Action? LogsChanged;
	List<Logs> Logs { get; set; }
	Task GetAllLogs();
}
