using System.Text.Json.Serialization;

namespace ShiftsLogger.Client.Models;

public class Shift
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }
    [JsonPropertyName("Employee")]
    public string Employee { get; set; } = string.Empty;
    [JsonPropertyName("Start")]
    public DateTime Start { get; set; }
    [JsonPropertyName("End")]
    public DateTime? End { get; set; }
    [JsonPropertyName("Duration")]
    public TimeSpan? Duration { get; set; }

}