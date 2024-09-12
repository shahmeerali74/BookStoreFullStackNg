using System;

namespace BookStoreFullStackNg.Data.DTOs.Author;

public class AuthorReadDTO
{
    public int Id { get; set; }

    public string AuthorName { get; set; } = string.Empty;
}
