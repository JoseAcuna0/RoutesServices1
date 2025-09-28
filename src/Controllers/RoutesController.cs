using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RoutesService.src.DTOs;
using RoutesService.src.Interfaces;

namespace RoutesService.src.Controllers
{

    /// <summary>
    /// Controlador encargado de exponer los endpoints para gestionar rutas de trenes.
    /// Permite crear, consultar, actualizar y eliminar rutas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        /// <summary>
        /// Servicio de rutas inyectado (maneja la lógica de negocio y conexión con Neo4j).
        /// </summary>
        private readonly IRouteService _routeService;

        /// <summary>
        /// Constructor del controlador de rutas.
        /// </summary>
        /// <param name="routeService">Servicio que implementa la lógica de rutas.</param>
        public RoutesController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        /// <summary>
        /// Crea una nueva ruta.
        /// </summary>
        /// <param name="dto">Objeto con los datos de la ruta: origen, destino, hora de inicio, fin y paradas.</param>
        /// <returns>Devuelve la ruta creada con su ID generado.</returns>
        /// <response code="201">Ruta creada exitosamente</response>
        /// <response code="400">Datos inválidos</response>
        [HttpPost]
        public async Task<IActionResult> CreateRoute([FromBody] RouteDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Origin) || string.IsNullOrEmpty(dto.Destination))
            {
                return BadRequest("Invalid route data.");
            }

            var createdRoute = await _routeService.CreateRouteAsync(dto);
            return CreatedAtAction(nameof(GetRouteById), new { id = createdRoute.Id }, createdRoute);
        }

        /// <summary>
        /// Obtiene todas las rutas registradas en el sistema.
        /// </summary>
        /// <returns>Lista de rutas existentes.</returns>
        [HttpGet]
        public async Task<IActionResult> GetRoutes()
        {
            var routes = await _routeService.GetRoutesAsync();
            return Ok(routes);
        }

        /// <summary>
        /// Busca una ruta específica por su ID.
        /// </summary>
        /// <param name="id">Identificador único de la ruta.</param>
        /// <returns>Ruta encontrada o error 404 si no existe.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRouteById(string id)
        {
            var route = await _routeService.GetRouteByIdAsync(id);
            if (route == null)
            {
                return NotFound();
            }
            return Ok(route);
        }

        /// <summary>
        /// Actualiza los datos de una ruta existente.
        /// </summary>
        /// <param name="id">Identificador único de la ruta a actualizar.</param>
        /// <param name="dto">Objeto con los nuevos datos de la ruta.</param>
        /// <returns>Ruta actualizada o error 404 si no existe.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoute(string id, [FromBody] RouteDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Origin) || string.IsNullOrEmpty(dto.Destination))
            {
                return BadRequest("Invalid route data.");
            }

            var updatedRoute = await _routeService.UpdateRouteAsync(id, dto);
            if (updatedRoute == null)
            {
                return NotFound();
            }
            return Ok(updatedRoute);
        }

        /// <summary>
        /// Elimina (soft delete) una ruta, marcándola como inactiva.
        /// </summary>
        /// <param name="id">Identificador único de la ruta a eliminar.</param>
        /// <returns>Código 204 si se eliminó, 404 si no se encontró.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoute(string id)
        {
            var result = await _routeService.DeleteRouteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}