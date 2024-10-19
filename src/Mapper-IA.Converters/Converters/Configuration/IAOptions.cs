using System.Text.Json;

namespace ConvertersIA.Converters.Configuration;

public class IAOptions
{
    public string Key { get; set; }
    public JsonSerializerOptions? jsonSerializerOptions { get; set; }

    
}