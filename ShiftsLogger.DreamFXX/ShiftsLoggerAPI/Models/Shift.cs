namespace ShiftsLoggerAPI.Models;    

public class Shift
{
    public int Id { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Notes { get; set; } = string.Empty;

    public TimeSpan Duration => EndTime - StartTime;
}
