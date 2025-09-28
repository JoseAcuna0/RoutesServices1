using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoutesService.src.DTOs;

namespace RoutesService.src.Interfaces
{
    /// <summary>
    /// Define el contrato para los servicios de gestión de rutas.
    /// Contiene las operaciones principales de creación, consulta, actualización y eliminación.
    /// </summary>
    public interface IRouteService
    {
        /// <summary>
        /// Crea una nueva ruta en el sistema.
        /// </summary>
        /// <param name="dto">Objeto con la información de la ruta a crear.</param>
        /// <returns>Objeto de respuesta con los datos de la ruta creada.</returns>
        Task<RouteResponseDto> CreateRouteAsync(RouteDto dto);

        /// <summary>
        /// Obtiene todas las rutas registradas en el sistema.
        /// </summary>
        /// <returns>Colección de rutas activas o registradas.</returns>
        Task<IEnumerable<RouteResponseDto>> GetRoutesAsync();

        /// <summary>
        /// Busca una ruta por su identificador único.
        /// </summary>
        /// <param name="id">Identificador de la ruta.</param>
        /// <returns>Ruta encontrada o null si no existe.</returns>
        Task<RouteResponseDto?> GetRouteByIdAsync(string id);

        /// <summary>
        /// Actualiza los datos de una ruta existente.
        /// </summary>
        /// <param name="id">Identificador de la ruta a actualizar.</param>
        /// <param name="dto">Objeto con los nuevos valores de la ruta.</param>
        /// <returns>Ruta actualizada o null si no existe.</returns>
        Task<RouteResponseDto?> UpdateRouteAsync(string id, RouteDto dto);

        /// <summary>
        /// Realiza la eliminación lógica (soft delete) de una ruta, marcándola como inactiva.
        /// </summary>
        /// <param name="id">Identificador de la ruta a eliminar.</param>
        /// <returns>True si la operación fue exitosa, False si no se encontró la ruta.</returns>
        Task<bool> DeleteRouteAsync(string id);
    }
}