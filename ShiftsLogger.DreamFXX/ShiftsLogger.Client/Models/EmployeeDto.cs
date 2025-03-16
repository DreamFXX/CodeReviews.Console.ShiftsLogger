using System.Text.Json.Serialization;

namespace ShiftsLogger.Client.Models;

public class EmployeeDto
{
    [JsonPropertyName("workerName")]
    public string EmployeeName { get; set; } = string.Empty;
}
