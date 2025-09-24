using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RoutesService.src.DTOs;
using RoutesService.src.Interfaces;

namespace RoutesService.src.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public RoutesController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        //Crear Ruta
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

        //Obtener todas las rutas
        [HttpGet]
        public async Task<IActionResult> GetRoutes()
        {
            var routes = await _routeService.GetRoutesAsync();
            return Ok(routes);
        }

        //Obtener ruta por ID
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

        //Editar ruta
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

        //Eliminar ruta (Soft delete)
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