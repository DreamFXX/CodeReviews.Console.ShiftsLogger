using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShiftsLogger.Api.Models;

public class EmployeeDto
{
    [Required(ErrorMessage = "Name of employee is required!")]
    [JsonPropertyName("employeeName")]
    public string EmployeeName { get; set; } = string.Empty;
}
