﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorEcommerce.Shared.Models;
public class Product
{
	[Key]
	public int Id { get; set; }
	[Required]
	//[MaxLength(50)]
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string ImageUrl { get; set; } = string.Empty;
	public List<Image> Images { get; set; } = new List<Image>();
    [ForeignKey("CategoryId")]
	public Category? Category { get; set; }
	public int CategoryId { get; set; }

	public bool Featured { get; set; } = false;
	public IEnumerable<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
	public IEnumerable<ProductRatings> Ratings { get; set; } = new List<ProductRatings>();

	public bool Visible { get; set; } = true;
	public bool Deleted { get; set; } = false;

	[NotMapped]
	public bool Editing { get; set; } = false;
	[NotMapped]
	public bool IsNew { get; set; } = false;
}
