using BookStoreFullStackNg.Data.Reopositories.Implementations;
using BookStoreFullStackNg.Data.Reopositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BookStoreFullStackNg.Data.Extensions;

public static class ServiceExtension
{
    public static void RegisterDataServices(this IServiceCollection services)
    {
        services.AddTransient<IGenreRepository, GenreRepository>();
    }
}
