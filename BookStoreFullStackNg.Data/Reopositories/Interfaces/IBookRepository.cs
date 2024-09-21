using BookStoreFullStackNg.Data.Domain;
using BookStoreFullStackNg.Data.DTOs.Book;

namespace BookStoreFullStackNg.Data.Reopositories.Interfaces
{
    public interface IBookRepository
    {
        Task<BookReadDto> AddBookAsync(BookCreateDto bookToCreate);
        Task DeleteBookAsync(Book book);
        Task<BookReadDto?> GetBookByIdAsync(int bookId);
        Task<IEnumerable<BookReadDto>> GetBooksAsync(int bookId);
        Task UpdateBookAsync(BookUpdateDto bookToUpdate);
    }
}