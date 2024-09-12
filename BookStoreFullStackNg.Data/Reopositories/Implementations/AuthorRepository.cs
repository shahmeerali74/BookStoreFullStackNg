using System;
using BookStoreFullStackNg.Data.Data;
using BookStoreFullStackNg.Data.Domain;
using BookStoreFullStackNg.Data.DTOs.Author;
using BookStoreFullStackNg.Data.DTOs.Common;
using BookStoreFullStackNg.Data.Reopositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStoreFullStackNg.Data.Reopositories.Implementations;

public class AuthorRepository : IAuthorRepository
{
    private readonly BookStoreContext _bookStoreContext;

    public AuthorRepository(BookStoreContext bookStoreContext)
    {
        _bookStoreContext = bookStoreContext;
    }
    public async Task<Author> CreateAuthor(Author author)
    {
        if (author == null)
        {
            throw new ArgumentNullException(nameof(author));
        }
        _bookStoreContext.Authors.Add(author);
        await _bookStoreContext.SaveChangesAsync();
        return author;
    }

    public async Task UpdateAuthor(Author author)
    {
        if (author == null)
        {
            throw new ArgumentNullException(nameof(author));
        }
        _bookStoreContext.Authors.Update(author);
        await _bookStoreContext.SaveChangesAsync();
    }

    public async Task DeleteAuthor(Author author)
    {
        if (author == null)
        {
            throw new ArgumentNullException(nameof(author));
        }
        _bookStoreContext.Remove(author);
        await _bookStoreContext.SaveChangesAsync();
    }

    public async Task<Author?> GetAuthor(int id)
    {
        return await _bookStoreContext.Authors.Where(a => a.Id == id).AsNoTracking().FirstOrDefaultAsync();
    }

    public async Task<PagedList<Author>> GetAuthors(AuthorQueryParameters queryParameters)
    {
        IQueryable<Author> authorsQuery = _bookStoreContext.Authors.AsNoTracking();

        // filter by search term
        if (!string.IsNullOrEmpty(queryParameters.SearchTerm))
        {
            authorsQuery = authorsQuery.Where(a => a.AuthorName.ToLower().Contains(queryParameters.SearchTerm));
        }

        if (!string.IsNullOrEmpty(queryParameters.SortBy))
        {
            bool isDescending = queryParameters.SortOrder == "desc";
            switch (queryParameters.SortBy)
            {
                case "authorname":
                    authorsQuery = isDescending ? authorsQuery.OrderByDescending(a => a.AuthorName) : authorsQuery.OrderBy(a => a.AuthorName);
                    break;
                default:
                    authorsQuery = authorsQuery.OrderBy(a => a.AuthorName);
                    break;
            }
        }

        return await PagedList<Author>.ToPagedListAsync(authorsQuery, queryParameters.PageNumber, queryParameters.PageSize);


    }


}
