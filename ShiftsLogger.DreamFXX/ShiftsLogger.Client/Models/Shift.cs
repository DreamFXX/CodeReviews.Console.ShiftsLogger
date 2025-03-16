using System.Text.Json.Serialization;

namespace ShiftsLogger.Client.Models;

public class Shift
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("employeeName")]
    public string Employee { get; set; } = string.Empty;
    [JsonPropertyName("startTime")]
    public DateTime Start { get; set; }
    [JsonPropertyName("endTime")]
    public DateTime? End { get; set; }
    [JsonPropertyName("duration")]
    public TimeSpan? Duration { get; set; }
}