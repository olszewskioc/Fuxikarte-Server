using Fuxikarte.Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Scalar.AspNetCore;
using Fuxikarte.Backend.Services;
using Fuxikarte.Backend.Mappings;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Fuxikarte.Backend;
using Fuxikarte.Backend.Extensions;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

#region BUILDER

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddServicesFromNamespace(
    typeof(UserService).Assembly,
    "Fuxikarte.Backend.Services"
);

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"))
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information)
);

var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key não configurado!");
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.ASCII.GetBytes(jwtKey);
        options.SaveToken = true;
        options.RequireHttpsMetadata = false; // ⚠️ só deixe false em dev
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Vite
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

#endregion

var app = builder.Build();

#region MIGRATION COM RETRY
// Observação: este bloco roda **antes** do servidor iniciar. Ele tenta aplicar
// migrations até 'maxAttempts' vezes, com backoff exponencial (limitado).
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var dbContext = services.GetRequiredService<AppDbContext>();

    const int maxAttempts = 10;
    for (int attempt = 1; attempt <= maxAttempts; attempt++)
    {
        try
        {
            logger.LogInformation("Tentando aplicar migrations (tentativa {Attempt}/{MaxAttempts})...", attempt, maxAttempts);
            dbContext.Database.Migrate(); // aplica migrations pendentes (síncrono)
            logger.LogInformation("Migrations aplicadas com sucesso.");
            break;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Falha ao aplicar migrations na tentativa {Attempt}.", attempt);
            if (attempt == maxAttempts)
            {
                logger.LogError(ex, "Não foi possível aplicar migrations após {MaxAttempts} tentativas. Encerrando aplicação.", maxAttempts);
                throw; // aborta a inicialização (você pode optar por não lançar e continuar)
            }
            // backoff simples (2s * attempt), limitado a 30s
            var delay = TimeSpan.FromSeconds(Math.Min(30, 2 * attempt));
            logger.LogInformation("Aguardando {Delay} antes da próxima tentativa...", delay);
            // usa Task.Delay para não bloquear a thread
            await Task.Delay(delay);
        }
    }
}
#endregion

#region APP

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok("Healthy")).ExcludeFromDescription();

// Redireciona raiz para Scalar
app.MapGet("/", () => Results.Redirect("/scalar/v1"))
   .ExcludeFromDescription();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Minha API - Fuxikarte")
            .WithTheme(ScalarTheme.BluePlanet)
            .WithDownloadButton(true)
            .WithPreferredScheme("Bearer");
        options.ShowSidebar = true;
    });
}

// Middleware de log simples
app.Use(async (context, next) =>
{
    Console.WriteLine(
        $"[{DateTime.Now}] {context.Request.Method} {context.Request.Path} | " +
        $"User: {context.User.Identity?.Name ?? "anônimo"} | " +
        $"Authenticated: {context.User.Identity?.IsAuthenticated}"
    );
    await next();
});

// NOTA: RunAsync porque o bloco de migrations usa await
await app.RunAsync();

#endregion
