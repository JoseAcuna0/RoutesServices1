using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoutesService.src.DTOs
{
    /// <summary>
    /// Objeto de transferencia de datos (DTO) utilizado para devolver información de las rutas.
    /// Representa la respuesta que el cliente recibe al consultar o crear rutas.
    /// </summary>
    public class RouteResponseDto
    {
        /// <summary>
        /// Identificador único de la ruta.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Estación de origen de la ruta.
        /// </summary>
        public string Origin { get; set; } = string.Empty;

        /// <summary>
        /// Estación de destino de la ruta.
        /// </summary>
        public string Destination { get; set; } = string.Empty;

        /// <summary>
        /// Hora de inicio del recorrido (formato HH:mm).
        /// </summary>
        public string StartTime { get; set; } = string.Empty;

        /// <summary>
        /// Hora de término del recorrido (formato HH:mm).
        /// </summary>
        public string EndTime { get; set; } = string.Empty;

        /// <summary>
        /// Lista de paradas intermedias de la ruta.
        /// </summary>
        public List<string> Stops { get; set; } = new();

        /// <summary>
        /// Estado de la ruta (true = activa, false = inactiva).
        /// </summary>
        public bool IsActive { get; set; }
    }
}
