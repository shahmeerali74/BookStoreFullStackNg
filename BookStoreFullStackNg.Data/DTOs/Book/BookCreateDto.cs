using System.ComponentModel.DataAnnotations;
using BookStoreFullStackNg.Data.Domain;

namespace BookStoreFullStackNg.Data.DTOs.Book;

public class BookCreateDto
{
    [Required]
    [MaxLength(30)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;
    public double Price { get; set; }
    public string? ImageUrl { get; set; }
    public int PublishedYear { get; set; }

    [Required]
    public List<int> AuthorIds { get; set; } = [];
    [Required]
    public List<int> GenreIds { get; set; } = [];
}
