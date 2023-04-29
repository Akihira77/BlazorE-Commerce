namespace BlazorEcommerce.Shared.Models;
public class Logs
{
	public int Id { get; set; }
	public string LogEvent { get; set; } = string.Empty;
	public DateTime LogCreated { get; set; } = DateTime.Now;
}
