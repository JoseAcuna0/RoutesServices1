using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoutesService.src.DTOs;
using RoutesService.src.Models;

namespace RoutesService.src.Mappers
{
    public class RouteMapper
    {
        public static TrainRoute ToModel(RouteDto dto) =>
        new TrainRoute
        {
            Origin = dto.Origin,
            Destination = dto.Destination,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Stops = dto.Stops
        };

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