namespace MonitoringAPI.Models;

public class Device
{
    public string Id { get; set; } // Guid??
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Version { get; set; }
}