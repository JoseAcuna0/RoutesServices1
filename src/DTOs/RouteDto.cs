using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoutesService.src.DTOs
{

    /// <summary>
    /// Objeto de transferencia de datos (DTO) utilizado para crear o actualizar rutas.
    /// Contiene la información básica que describe una ruta dentro del sistema.
    /// </summary>
    public class RouteDto
    {
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
        /// Lista de paradas intermedias incluidas en el trayecto.
        /// </summary>
        public List<string> Stops { get; set; } = new();

        /// <summary>
        /// Estado de la ruta (true = activa, false = inactiva).
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}