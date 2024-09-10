using System;
using BookStoreFullStackNg.Data.Data;
using BookStoreFullStackNg.Data.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace BookStoreFullStackNg.IntegratedTests;

public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
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

            services.AddDbContext<BookStoreContext>(options => options.UseInMemoryDatabase("InMem"));

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

    private static void SeedData(BookStoreContext context)
    {
        if (!context.Genres.Any())
        {
            var genres = new List<Genre>{
                new Genre{Id=1,GenreName="G1"},
                new Genre{Id=2,GenreName="G2"},
            };

            context.Genres.AddRange(genres);
            context.SaveChanges();
        }
    }
}
