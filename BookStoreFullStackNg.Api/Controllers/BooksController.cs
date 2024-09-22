using AutoMapper;
using BookStoreFullStackNg.Api.Exceptions;
using BookStoreFullStackNg.Data.Domain;
using BookStoreFullStackNg.Data.DTOs;
using BookStoreFullStackNg.Data.DTOs.Author;
using BookStoreFullStackNg.Data.DTOs.Book;
using BookStoreFullStackNg.Data.DTOs.Common;
using BookStoreFullStackNg.Data.Reopositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreFullStackNg.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IBookRepository _bookRepo;
    public BooksController(IMapper mapper, IBookRepository bookRepo)
    {
        _mapper = mapper;
        _bookRepo = bookRepo;
    }

    [HttpPost]
    public async Task<IActionResult> AddBook(BookCreateDto bookCreateDto)
    {
        BookReadDto createdBook= await _bookRepo.AddBookAsync(bookCreateDto);
        return CreatedAtRoute(nameof(GetBookById),new { id=createdBook.Id},createdBook);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id,BookUpdateDto bookToUpdate)
    {
        if(id!=bookToUpdate.Id)
        {
            throw new BadRequestException($"Id mismatch");
        }

        var existingBook = await _bookRepo.GetBookByIdAsync(id);
        if(existingBook==null)
        {
            throw new NotFoundException($"Book with id:{id} does not found");
        }
        await _bookRepo.UpdateBookAsync(bookToUpdate);
        return NoContent();
    }


    [HttpGet]
    public async Task<IActionResult> GetBooks([FromQuery]BookQueryParameter queryParameter)
    {
        var pagedBooks = await _bookRepo.GetBooksAsync(queryParameter); // have related data
        var books = pagedBooks.Items.Select(b =>
            new BookReadDto
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                Price = b.Price,
                ImageUrl = b.ImageUrl,
                PublishedYear = b.PublishedYear,
                Authors = b.BookAuthors.Select(ba => new AuthorReadDTO(ba.Author.Id, ba.Author.AuthorName)).ToList(),
                Genres = b.BookGenres.Select(bg => new GenreReadDto(bg.Genre.Id, bg.Genre.GenreName)).ToList()
            }).ToList() ;
        var newBookPagedList = new PagedList<BookReadDto>(books, pagedBooks.TotalCount, pagedBooks.PageNumber, pagedBooks.PageSize);
        return Ok(newBookPagedList);
    }

    [HttpGet("{id}",Name =nameof(GetBookById))]
    public async Task<IActionResult> GetBookById(int id)
    {
        BookReadDto? book = await _bookRepo.GetBookByIdAsync(id);
        if(book==null)
        {
            throw new NotFoundException($"Book with id:{id} does not found");
        }
        return Ok(book);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        BookReadDto? existingBook = await _bookRepo.GetBookByIdAsync(id);
        if (existingBook == null)
        {
            throw new NotFoundException($"Book with id:{id} does not found");
        }
        Book book = _mapper.Map<Book>(existingBook);
        await _bookRepo.DeleteBookAsync(book);
        return NoContent() ;
    }
}
