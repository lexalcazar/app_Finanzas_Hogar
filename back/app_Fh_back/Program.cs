using Microsoft.EntityFrameworkCore;
using app_Fh_back.Data;
using app_Fh_back.Models;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using app_Fh_back.Services;
using app_Fh_back.Configuration;
using app_Fh_back.Services.IA;

var builder = WebApplication.CreateBuilder(args);

// Controladores
builder.Services.AddControllers();

// OpenAPI
builder.Services.AddOpenApi();

// Conexión PostgreSQL
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Identity
builder.Services
    .AddIdentity<Usuario, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException(
        "No se ha configurado la clave JWT.");
}

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey))
            };
    });

builder.Services.AddAuthorization();
// Servicios
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<MovimientoService>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<IAsistenteService, AsistenteService>();
// Configuración de LmStudio
builder.Services.Configure<LmStudioOptions>(
    builder.Configuration.GetSection(LmStudioOptions.SectionName)
);

builder.Services.AddHttpClient<ILmStudioService, LmStudioService>();

var app = builder.Build();

// OpenAPI + Scalar solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Inicializar roles
using (var scope = app.Services.CreateScope())
{
    await DbInitializer.InicializarAsync(scope.ServiceProvider);
}

app.UseHttpsRedirection();

// Para Identity
app.UseAuthentication();
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

app.Run();