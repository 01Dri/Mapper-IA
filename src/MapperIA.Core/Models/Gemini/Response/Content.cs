using System.Text.Json.Serialization;
using MapperIA.Core.Models.Gemini.Request;

namespace MapperIA.Core.Models.Gemini.Response;

public class Content
{
    [JsonPropertyName("parts")]
    public List<Parts> Parts { get; set; }
}