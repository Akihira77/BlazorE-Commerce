using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerce.Shared;
public class Category
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}
