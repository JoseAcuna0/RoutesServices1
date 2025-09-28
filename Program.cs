using Neo4j.Driver;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RoutesService.src.Interfaces;
using RoutesService.src.Services;
using DotNetEnv;

/// <summary>
/// Clase principal de arranque de la aplicación.
/// Configura los servicios, dependencias e inicializa la API.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Cargar variables de entorno desde el archivo .env.
/// Esto permite mantener seguras las credenciales de Neo4j.
/// </summary>
Env.Load();

/// <summary>
/// Configuración de la conexión a Neo4j.
/// Se priorizan las variables de entorno (Neo4j__Uri, Neo4j__Username, Neo4j__Password),
/// pero también se leen valores desde appsettings.json como respaldo.
/// </summary>
var neo4jSettings = builder.Configuration.GetSection("Neo4j");
var driver = GraphDatabase.Driver(
    builder.Configuration["Neo4j__Uri"] ?? neo4jSettings["Uri"],
    AuthTokens.Basic(
        builder.Configuration["Neo4j__Username"] ?? neo4jSettings["Username"],
        builder.Configuration["Neo4j__Password"] ?? neo4jSettings["Password"]
    )
);

/// <summary>
/// Registro de dependencias en el contenedor de servicios:
/// - Driver de Neo4j como Singleton.
/// - RouteService como implementación de IRouteService.
/// - Soporte para controladores y Swagger.
/// </summary>
builder.Services.AddSingleton(driver);
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/// <summary>
/// Construcción de la aplicación.
/// </summary>
var app = builder.Build();

/// <summary>
/// Mapeo de controladores de la API.
/// </summary>
app.MapControllers();

/// <summary>
/// Configuración del pipeline HTTP.
/// Incluye Swagger en entorno de desarrollo y redirección a HTTPS.
/// </summary>
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/// <summary>
/// Punto de arranque de la aplicación.
/// </summary>
app.Run();
