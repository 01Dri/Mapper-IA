using System.Text.Json.Serialization;

namespace MapperIA.Core.Models.Gemini.Response;

public class CandidatesResponse
{
    [JsonPropertyName("content")] public Content Content { get; set; }
}