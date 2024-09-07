using BookStoreFullStackNg.Api.Middlewares;
using BookStoreFullStackNg.Data.Data;
using BookStoreFullStackNg.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

var allowedOrigins = "_allowedOrigins";
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
   .CreateLogger();

Log.Logger.Information("Logging is working fine");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterDataServices();
builder.Services.AddDbContext<BookStoreContext>(options => options.UseSqlite("Data source= BookStore.db"));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOrigins,
    policy =>
    {
        policy.WithOrigins("*").WithHeaders("*").WithMethods("*");
    });
});
builder.Services.AddTransient<ExceptionMiddleware>();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(allowedOrigins);

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();

public partial class Program { }