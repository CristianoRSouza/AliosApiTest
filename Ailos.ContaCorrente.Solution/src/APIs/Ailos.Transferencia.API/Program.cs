using Ailos.Transferencia.Infrastructure.Data;
using Ailos.Transferencia.API.Services;
using Ailos.Shared.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Data.Sqlite;
using System.Text;
using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurar Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ailos Transferência API", Version = "v1" });
    
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
var connection = new SqliteConnection(connectionString);
connection.Open();

builder.Services.AddSingleton(connection);
builder.Services.AddDbContext<TransferenciaDbContext>(options =>
{
    options.UseSqlite(connection);
    options.EnableSensitiveDataLogging();
});

// Registrar MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(Ailos.Transferencia.Application.Commands.EfetuarTransferencia.EfetuarTransferenciaCommand).Assembly
));

// Registrar serviços
builder.Services.AddScoped<Ailos.Transferencia.Domain.Interfaces.ITransferenciaRepository, Ailos.Transferencia.Infrastructure.Data.Repositories.TransferenciaRepository>();
builder.Services.AddScoped<MovimentacaoService>();
builder.Services.AddScoped<PasswordHashService>();

// Mock do Kafka Producer - SEM tentar conectar
builder.Services.AddSingleton<IProducer<string, string>>(provider => new MockKafkaProducer());

var contaCorrenteUrl = builder.Configuration["ContaCorrenteAPI:BaseUrl"] ?? "http://localhost:60850";
builder.Services.AddHttpClient("ContaCorrenteAPI", client =>
{
    client.BaseAddress = new Uri(contaCorrenteUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

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
    var context = scope.ServiceProvider.GetRequiredService<TransferenciaDbContext>();
    context.Database.EnsureCreated();
}

// Habilitar Swagger sempre
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ailos Transferência API v1");
    c.RoutePrefix = "swagger";
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
