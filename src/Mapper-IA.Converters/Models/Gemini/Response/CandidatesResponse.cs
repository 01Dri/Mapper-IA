using System.Text.Json.Serialization;

public class CandidatesResponse
{
    [JsonPropertyName("content")] public Content Content { get; set; }
}