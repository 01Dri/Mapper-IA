using System.Text.Json.Serialization;
using ConvertersIA.Models.Gemini.Request;

public class Content
{
    [JsonPropertyName("parts")]
    public List<Parts> Parts { get; set; }
}