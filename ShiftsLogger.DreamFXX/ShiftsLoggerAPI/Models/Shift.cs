using System.ComponentModel.DataAnnotations;

namespace ShiftsLoggerAPI.Models;

public class Shift
{
    [Key]
    public int Id { get; set; }
    public string? EmployeeName { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public DateTime CompletedWork { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
    public bool IsCompleted { get; set; }
}

