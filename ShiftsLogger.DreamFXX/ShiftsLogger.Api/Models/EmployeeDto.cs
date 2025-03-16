using System.Text.Json.Serialization;

namespace ShiftsLogger.Api.Models;

public class EmployeeDto
{
    [JsonPropertyName("employeeName")]
    public string? EmployeeName { get; set; }
}
