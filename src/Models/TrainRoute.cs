using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoutesService.src.Models
{
    /// <summary>
    /// Representa la entidad de una ruta de tren dentro del dominio de la aplicación.
    /// Este modelo corresponde a un nodo <c>(:Route)</c> almacenado en Neo4j.
    /// </summary>
    public class TrainRoute
    {
        /// <summary>
        /// Identificador único de la ruta (se genera automáticamente con un GUID).
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

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
        /// Lista de paradas intermedias incluidas en la ruta.
        /// </summary>
        public List<string> Stops { get; set; } = new();

        /// <summary>
        /// Estado de la ruta (true = activa, false = inactiva).
        /// Usado para implementar "soft delete".
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
