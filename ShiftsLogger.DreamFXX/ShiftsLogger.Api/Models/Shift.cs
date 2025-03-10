using Microsoft.EntityFrameworkCore;

namespace ShiftsLogger.Api.Models;
public class Shift
{
    public int Id { get; set; }
    public string WorkerName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }

    // Vypočítaná vlastnost pro celkovou dobu směny v hodinách
    public double DurationInHours => (EndTime - StartTime).TotalHours;

    // Vypočítaná vlastnost pro celkovou mzdu za směnu
    public decimal TotalPay => (decimal)DurationInHours * HourlyRate;
}
