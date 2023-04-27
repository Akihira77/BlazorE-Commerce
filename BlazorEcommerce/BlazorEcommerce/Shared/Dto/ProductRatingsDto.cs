namespace BlazorEcommerce.Shared.Dto;
public class ProductRatingsDto
{
    public int Rate { get; set; }
    public string? TextReviews { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
}
