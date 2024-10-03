using BookStoreFullStackNg.Data.Data;
using BookStoreFullStackNg.Data.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace BookStoreFullStackNg.IntegratedTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BookStoreContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<BookStoreContext>(options => {
                options.UseInMemoryDatabase("InMem");
                options.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            }
               );


            // Add a test authentication scheme
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "TestScheme";
                options.DefaultChallengeScheme = "TestScheme";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });



            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<BookStoreContext>();
            try
            {
                appContext.Database.EnsureDeleted();
                appContext.Database.EnsureCreated();
                SeedData(appContext);
            }
            catch (Exception ex)
            {
                throw;
            }
        });
    }

    //private static void DisposeExisitingData(BookStoreContext context)
    //{
    //    context.Genres.RemoveRange(context.Genres);
    //    context.Authors.RemoveRange(context.Authors);
    //    context.Books.RemoveRange(context.Books);
    //    context.BookAuthors.RemoveRange(context.BookAuthors);
    //    context.BookGenres.RemoveRange(context.BookGenres);
    //    context.SaveChanges();
    //}
    private static void SeedData(BookStoreContext context)
    {
        if (!context.Genres.Any())
        {
            var genres = new List<Genre>{
                new Genre{GenreName="G1"},
                new Genre{GenreName="G2"},
            };

            context.Genres.AddRange(genres);
        }
        if (!context.Authors.Any())
        {
            var authors = new List<Author> {
              new Author {AuthorName="A1"},
              new Author {AuthorName="A2"},
              new Author {AuthorName="A3"},
            };
            context.Authors.AddRange(authors);
        }
        if (!context.Books.Any())
        {
            var books = new List<Book>
             {
    new Book
    {
        Id = 1,
        Title = "book1",
        Price = 123,
        Description = "desc1",
        PublishedYear = 2008,
        ImageUrl = null,
        BookAuthors = new List<BookAuthor>
        {
            new BookAuthor { Id = 1, BookId = 1, AuthorId = 1 }
        },
        BookGenres = new List<BookGenre>
        {
            new BookGenre { Id = 1, BookId = 1, GenreId = 1 }
        }
    },
    new Book
    {
        Id = 2,
        Title = "book2",
        Price = 120,
        Description = "desc2",
        PublishedYear = 2009,
        ImageUrl = null,
        BookAuthors = new List<BookAuthor>
        {
            new BookAuthor { Id = 2, BookId = 2, AuthorId = 2 }
        },
        BookGenres = new List<BookGenre>
        {
            new BookGenre { Id = 2, BookId = 2, GenreId = 2 }
        }
    }
};
            context.Books.AddRange(books);
        }
        context.SaveChanges();


    }
}
