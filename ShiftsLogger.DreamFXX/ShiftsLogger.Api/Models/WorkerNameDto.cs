using System.Text.Json.Serialization;

namespace ShiftsLogger.Api.Models;

public class EmployeeDto
{
    [JsonPropertyName("workerName")]
    public string? EmployeeName { get; set; }
}
