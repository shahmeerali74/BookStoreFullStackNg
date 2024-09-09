using BookStoreFullStackNg.Data.Reopositories.Implementations;
using BookStoreFullStackNg.Data.Reopositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using BookStoreFullStackNg.Data.Services;

namespace BookStoreFullStackNg.Data.Extensions;

public static class ServiceExtension
{
    public static void RegisterDataServices(this IServiceCollection services)
    {
        services.AddTransient<IGenreRepository, GenreRepository>();
        services.AddTransient<IAuthService, AuthService>();
    }
}
