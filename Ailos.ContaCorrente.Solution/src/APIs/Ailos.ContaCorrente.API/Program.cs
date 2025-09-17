using Ailos.ContaCorrente.Domain.Interfaces;
using Ailos.ContaCorrente.Infrastructure.Data;
using Ailos.ContaCorrente.Infrastructure.Data.Repositories;
using Ailos.ContaCorrente.Infrastructure.Cache;
using Ailos.ContaCorrente.API.Services;
using Ailos.Shared.Common.Interfaces;
using Ailos.Shared.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Data.Sqlite;
using System.Text;
using AppServices = Ailos.ContaCorrente.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurar Cache
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICacheService, Ailos.ContaCorrente.Infrastructure.Cache.CacheService>();

// Configurar Kafka (desabilitado)
// var kafkaBootstrapServers = builder.Configuration.GetValue<string>("Kafka:BootstrapServers") ?? "localhost:9092";
// builder.Services.AddSingleton<IEventPublisher>(provider => new KafkaEventPublisher(kafkaBootstrapServers));
builder.Services.AddSingleton<IEventPublisher, NoOpEventPublisher>();

// Configurar Swagger com JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ailos ContaCorrente API", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configurar banco SQLite em memória
var connectionString = "Data Source=:memory:";
var connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString);
connection.Open();

builder.Services.AddSingleton(connection);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(connection);
    options.EnableSensitiveDataLogging();
});

builder.Services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
builder.Services.AddScoped<IMovimentoRepository, MovimentoRepository>();
builder.Services.AddScoped<AppServices.IJwtService>(provider => 
{
    var jwtSecretKey = builder.Configuration.GetValue<string>("JWT:SecretKey") ?? "my-secret-key-32-characters-long";
    return new AppServices.JwtService(jwtSecretKey);
});
builder.Services.AddScoped<PasswordHashService>();

// Registrar MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Ailos.ContaCorrente.Application.Commands.CadastrarContaCorrente.CadastrarContaCorrenteCommand).Assembly));

// Configuração JWT
var jwtSecretKey = builder.Configuration.GetValue<string>("JWT:SecretKey") ?? "my-secret-key-32-characters-long";
builder.Services.AddAuthentication("Bearer")
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecretKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Redirecionar raiz para Swagger
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

// Criar e inicializar banco em memória
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Habilitar Swagger sempre (não só em Development)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ailos ContaCorrente API v1");
    c.RoutePrefix = "swagger";
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Necessário para testes de integração
public partial class Program { }
