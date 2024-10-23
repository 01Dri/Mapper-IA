using System.Text.Json.Serialization;

namespace MapperIA.Core.Models.Gemini.Request;

public class GeminiPromptRequest
{
    [JsonPropertyName("contents")] public List<Contents> Contents { get; set; }
}