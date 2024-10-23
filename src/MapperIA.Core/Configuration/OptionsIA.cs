using System.Text.Json;

namespace MapperIA.Core.Configuration;

public class OptionsIA
{
    public string? Key { get; set; }
    public JsonSerializerOptions? jsonSerializerOptions { get; set; }

    
}