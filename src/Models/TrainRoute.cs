using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoutesService.src.Models
{
    public class TrainRoute
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public List<string> Stops { get; set; } = new();
        public bool IsActive { get; set; } = true;
    }
}