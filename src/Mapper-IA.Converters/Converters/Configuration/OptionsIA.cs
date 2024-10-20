using System.Text.Json;

namespace ConvertersIA.Converters.Configuration;

public class OptionsIA
{
    public string? Key { get; set; }
    public JsonSerializerOptions? jsonSerializerOptions { get; set; }

    
}