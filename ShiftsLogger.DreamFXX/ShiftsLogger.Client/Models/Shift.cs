using System.Text.Json.Serialization;

namespace ShiftsLogger.Client.Models;

public class Shift
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("workerName")]
    public string workerName { get; set; } = string.Empty;
    [JsonPropertyName("start")]
    public DateTime Start { get; set; }
    [JsonPropertyName("end")]
    public DateTime? End { get; set; }
    [JsonPropertyName("duration")]
    public TimeSpan? Duration { get; set; }

}