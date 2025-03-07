using System.ComponentModel.DataAnnotations;

namespace ShiftsLoggerAPI.Models;
public class Shift
{
    public int ShiftId { get; set; }
    public string WorkerName { get; set; } = String.Empty;
    [Required]
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string Description { get; set; } = String.Empty;
}
