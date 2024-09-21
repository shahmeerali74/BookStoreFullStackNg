
using System.ComponentModel.DataAnnotations;

namespace BookStoreFullStackNg.Data.DTOs.Book;

public class BookUpdateDto
{
    public int Id { get; set; }
    [Required]
    [MaxLength(30)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;
    public double Price { get; set; }
    public string? ImageUrl { get; set; }
    public int PublishedYear { get; set; }

    public List<int> AuthorIds { get; set; } = [];
    public List<int> GenreIds { get; set; } = [];
}
