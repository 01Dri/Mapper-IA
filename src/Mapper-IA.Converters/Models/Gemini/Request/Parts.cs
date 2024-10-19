using System.Text.Json.Serialization;

namespace ConvertersIA.Models.Gemini.Request;

public class Parts
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
}