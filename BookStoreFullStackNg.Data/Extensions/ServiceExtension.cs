using BookStoreFullStackNg.Data.Reopositories.Implementations;
using BookStoreFullStackNg.Data.Reopositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using BookStoreFullStackNg.Data.Services;
using BookStoreFullStackNg.Data.Helpers;
using BookStoreFullStackNg.Data.Domain;

namespace BookStoreFullStackNg.Data.Extensions;

public static class ServiceExtension
{
    public static void RegisterDataServices(this IServiceCollection services)
    {
        services.AddTransient<IGenreRepository, GenreRepository>();
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IAuthorRepository, AuthorRepository>();
        services.AddTransient<ISortHelper<Author>, SortHelper<Author>>();
        services.AddTransient<IBookRepository, BookRepository>();
    }
}
