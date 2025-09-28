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
    /// <summary>
    /// Implementación del servicio de rutas.
    /// Contiene la lógica de negocio y las consultas a la base de datos Neo4j.
    /// </summary>
    public class RouteService : IRouteService
    {
        /// <summary>
        /// Driver de Neo4j utilizado para establecer la conexión con la base de datos.
        /// </summary>
        private readonly IDriver _driver;

        /// <summary>
        /// Constructor del servicio de rutas.
        /// </summary>
        /// <param name="driver">Driver de Neo4j inyectado desde la configuración.</param>
        public RouteService(IDriver driver)
        {
            _driver = driver;
        }

        /// <summary>
        /// Crea una nueva ruta en Neo4j, validando previamente duplicados.
        /// </summary>
        /// <param name="dto">Datos de la ruta enviados por el cliente.</param>
        /// <returns>Ruta creada en formato <see cref="RouteResponseDto"/>.</returns>
        /// <exception cref="InvalidOperationException">Si ya existe una ruta con los mismos datos.</exception>
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

        /// <summary>
        /// Obtiene todas las rutas registradas en la base de datos.
        /// </summary>
        /// <returns>Listado de rutas en formato <see cref="RouteResponseDto"/>.</returns>
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

        /// <summary>
        /// Busca una ruta específica por su identificador único.
        /// </summary>
        /// <param name="id">Identificador de la ruta.</param>
        /// <returns>Ruta encontrada o null si no existe.</returns>
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

        /// <summary>
        /// Actualiza los datos de una ruta existente.
        /// </summary>
        /// <param name="id">Identificador de la ruta.</param>
        /// <param name="dto">Datos nuevos de la ruta.</param>
        /// <returns>Ruta actualizada o null si no existe.</returns>
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
                    dto.IsActive // permite editar el estado
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

        /// <summary>
        /// Realiza la eliminación lógica (soft delete) de una ruta.
        /// </summary>
        /// <param name="id">Identificador de la ruta.</param>
        /// <returns>True si se encontró y eliminó la ruta, False en caso contrario.</returns>
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

                return records.Any(); // true si se encontró la ruta
            }
            finally
            {
                await session.CloseAsync();
            }
        }
    }
}
