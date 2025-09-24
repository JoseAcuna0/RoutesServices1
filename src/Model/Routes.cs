namespace RoutesService.Models;

public class Routes
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public List<string> Stops { get; set; } = new();
    public bool IsActive { get; set; } = true;
    
}
