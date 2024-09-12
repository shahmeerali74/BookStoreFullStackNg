using System.ComponentModel.DataAnnotations;

namespace BookStoreFullStackNg.Data.DTOs.Author;

public class AuthorReadDTO
{

    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string AuthorName { get; set; } = string.Empty;
}
