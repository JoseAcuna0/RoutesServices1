using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoutesService.src.DTOs;

namespace RoutesService.src.Interfaces
{
    public interface IRouteService
    {
        Task<RouteResponseDto> CreateRouteAsync(RouteDto dto);
        Task<IEnumerable<RouteResponseDto>> GetRoutesAsync();
        Task<RouteResponseDto?> GetRouteByIdAsync(string id);
        Task<RouteResponseDto?> UpdateRouteAsync(string id, RouteDto dto);
        Task<bool> DeleteRouteAsync(string id); // Soft delete
    }
}