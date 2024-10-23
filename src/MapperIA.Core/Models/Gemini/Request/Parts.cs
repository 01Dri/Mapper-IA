using System.Text.Json.Serialization;

namespace MapperIA.Core.Models.Gemini.Request;

public class Parts
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
}