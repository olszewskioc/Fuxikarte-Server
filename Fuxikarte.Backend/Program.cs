using Fuxikarte.Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Scalar.AspNetCore;
using Fuxikarte.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddScoped<UserService>();
// builder.Services.AddScoped<AuthService>();   
// builder.Services.AddScoped<TokenService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"))
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information));

var app = builder.Build();

app.MapGet("/", () => Results.Redirect("/scalar/v1")); // Redireciona a raiz para o Scalar

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Minha API - Fuxikarte")
            .WithTheme(ScalarTheme.Purple)
            .WithDownloadButton(true);
    });
}

app.UseDefaultFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
