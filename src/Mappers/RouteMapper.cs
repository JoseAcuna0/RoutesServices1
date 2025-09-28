using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoutesService.src.DTOs;
using RoutesService.src.Models;

namespace RoutesService.src.Mappers
{
    /// <summary>
    /// Clase encargada de realizar las conversiones entre los objetos de dominio (TrainRoute)
    /// y los objetos de transferencia de datos (DTOs).
    /// </summary>
    public class RouteMapper
    {
        /// <summary>
        /// Convierte un objeto <see cref="RouteDto"/> (entrada desde el cliente)
        /// en un objeto de dominio <see cref="TrainRoute"/> para ser persistido en Neo4j.
        /// </summary>
        /// <param name="dto">Datos de la ruta enviados por el cliente.</param>
        /// <returns>Objeto de tipo <see cref="TrainRoute"/> listo para guardarse en la base de datos.</returns>
        public static TrainRoute ToModel(RouteDto dto) =>
            new TrainRoute
            {
                Origin = dto.Origin,
                Destination = dto.Destination,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Stops = dto.Stops
            };

        /// <summary>
        /// Convierte un objeto <see cref="TrainRoute"/> (modelo de dominio)
        /// en un objeto <see cref="RouteResponseDto"/> para enviar al cliente.
        /// </summary>
        /// <param name="route">Entidad de ruta obtenida desde la base de datos.</param>
        /// <returns>Objeto de tipo <see cref="RouteResponseDto"/> con los datos de la ruta.</returns>
        public static RouteResponseDto ToDto(TrainRoute route) =>
            new RouteResponseDto
            {
                Id = route.Id,
                Origin = route.Origin,
                Destination = route.Destination,
                StartTime = route.StartTime,
                EndTime = route.EndTime,
                Stops = route.Stops,
                IsActive = route.IsActive
            };
    }
}
