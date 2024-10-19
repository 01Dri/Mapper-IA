using System.Text.Json.Serialization;

namespace ConvertersIA.Models.Gemini.Request;

public class Contents
{
    [JsonPropertyName("parts")] 
    public List<Parts> Parts { get; set; }
}