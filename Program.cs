
using Neo4j.Driver;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RoutesService.src.Interfaces;
using RoutesService.src.Services;


var builder = WebApplication.CreateBuilder(args);

var neo4jSettings = builder.Configuration.GetSection("Neo4j");
var driver = GraphDatabase.Driver(
    neo4jSettings["Uri"],
    AuthTokens.Basic(neo4jSettings["Username"], neo4jSettings["Password"]),
    config => config.WithEncryptionLevel(EncryptionLevel.None)
);
builder.Services.AddSingleton(driver);
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
