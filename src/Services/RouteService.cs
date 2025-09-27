using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;
using RoutesService.src.DTOs;
using RoutesService.src.Interfaces;
using RoutesService.src.Mappers;
using RoutesService.src.Models;

namespace RoutesService.src.Services
{
    public class RouteService : IRouteService
    {
        private readonly IDriver _driver;

        public RouteService(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<RouteResponseDto> CreateRouteAsync(RouteDto dto)
        {
            var route = RouteMapper.ToModel(dto);

            var session = _driver.AsyncSession();
            try
            {
                // 1. Validar duplicados antes de crear
                var checkQuery = @"
                    MATCH (r:Route)
                    WHERE r.Origin = $Origin 
                      AND r.Destination = $Destination 
                      AND r.StartTime = $StartTime 
                      AND r.EndTime = $EndTime
                    RETURN r
                ";

                var checkResult = await session.RunAsync(checkQuery, new
                {
                    route.Origin,
                    route.Destination,
                    route.StartTime,
                    route.EndTime
                });

                var duplicates = await checkResult.ToListAsync();
                if (duplicates.Any())
                {
                    throw new InvalidOperationException("Ya existe una ruta con los mismos datos (origen, destino, inicio y término).");
                }

                // 2. Crear ruta si no hay duplicados
                var createQuery = @"
                    CREATE (r:Route {
                        Id: $Id,
                        Origin: $Origin,
                        Destination: $Destination,
                        StartTime: $StartTime,
                        EndTime: $EndTime,
                        Stops: $Stops,
                        IsActive: $IsActive
                    })
                    RETURN r
                ";

                var result = await session.RunAsync(createQuery, new
                {
                    route.Id,
                    route.Origin,
                    route.Destination,
                    route.StartTime,
                    route.EndTime,
                    route.Stops,
                    route.IsActive
                });

                var record = await result.SingleAsync();
                var node = record["r"].As<INode>();

                return RouteMapper.ToDto(new TrainRoute
                {
                    Id = node["Id"].As<string>(),
                    Origin = node["Origin"].As<string>(),
                    Destination = node["Destination"].As<string>(),
                    StartTime = node["StartTime"].As<string>(),
                    EndTime = node["EndTime"].As<string>(),
                    Stops = node["Stops"].As<List<string>>(),
                    IsActive = node["IsActive"].As<bool>()
                });
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<IEnumerable<RouteResponseDto>> GetRoutesAsync()
        {
            var query = @"MATCH (r:Route) RETURN r";

            var session = _driver.AsyncSession();
            try
            {
                var result = await session.RunAsync(query);
                var routes = new List<RouteResponseDto>();

                await result.ForEachAsync(record =>
                {
                    var node = record["r"].As<INode>();
                    routes.Add(new RouteResponseDto
                    {
                        Id = node["Id"].As<string>(),
                        Origin = node["Origin"].As<string>(),
                        Destination = node["Destination"].As<string>(),
                        StartTime = node["StartTime"].As<string>(),
                        EndTime = node["EndTime"].As<string>(),
                        Stops = node["Stops"].As<List<string>>(),
                        IsActive = node["IsActive"].As<bool>()
                    });
                });

                return routes;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<RouteResponseDto?> GetRouteByIdAsync(string id)
        {
            var query = @"MATCH (r:Route {Id: $Id}) RETURN r";

            var session = _driver.AsyncSession();
            try
            {
                var result = await session.RunAsync(query, new { Id = id });
                var records = await result.ToListAsync();

                if (!records.Any())
                    return null;

                var node = records.First()["r"].As<INode>();

                return new RouteResponseDto
                {
                    Id = node["Id"].As<string>(),
                    Origin = node["Origin"].As<string>(),
                    Destination = node["Destination"].As<string>(),
                    StartTime = node["StartTime"].As<string>(),
                    EndTime = node["EndTime"].As<string>(),
                    Stops = node["Stops"].As<List<string>>(),
                    IsActive = node["IsActive"].As<bool>()
                };
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<RouteResponseDto?> UpdateRouteAsync(string id, RouteDto dto)
        {
            var query = @"
                MATCH (r:Route {Id: $Id})
                SET r.Origin = $Origin,
                    r.Destination = $Destination,
                    r.StartTime = $StartTime,
                    r.EndTime = $EndTime,
                    r.Stops = $Stops,
                    r.IsActive = $IsActive
                RETURN r
            ";

            var session = _driver.AsyncSession();
            try
            {
                var result = await session.RunAsync(query, new
                {
                    Id = id,
                    dto.Origin,
                    dto.Destination,
                    dto.StartTime,
                    dto.EndTime,
                    dto.Stops,
                    dto.IsActive // 👈 ahora se puede editar el estado
                });

                var records = await result.ToListAsync();
                if (!records.Any())
                    return null;

                var node = records.First()["r"].As<INode>();

                return new RouteResponseDto
                {
                    Id = node["Id"].As<string>(),
                    Origin = node["Origin"].As<string>(),
                    Destination = node["Destination"].As<string>(),
                    StartTime = node["StartTime"].As<string>(),
                    EndTime = node["EndTime"].As<string>(),
                    Stops = node["Stops"].As<List<string>>(),
                    IsActive = node["IsActive"].As<bool>()
                };
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<bool> DeleteRouteAsync(string id)
        {
            var query = @"
                MATCH (r:Route {Id: $Id})
                SET r.IsActive = false
                RETURN r
            ";

            var session = _driver.AsyncSession();
            try
            {
                var result = await session.RunAsync(query, new { Id = id });
                var records = await result.ToListAsync();

                return records.Any(); // 👈 corregido
            }
            finally
            {
                await session.CloseAsync();
            }
        }
    }
}