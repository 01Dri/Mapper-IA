using System.Text.Json.Serialization;

namespace MapperIA.Core.Models.Gemini.Request;

public class Contents
{
    [JsonPropertyName("parts")] 
    public List<Parts> Parts { get; set; }
}