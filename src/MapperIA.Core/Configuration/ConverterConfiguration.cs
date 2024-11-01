using System.Text.Json;

namespace MapperIA.Core.Configuration;

public class ConverterConfiguration
{
    public string? Key { get; set; }
    public JsonSerializerOptions? JsonSerializerOptions { get; set; }
    public string Model { get; set; } = "gemini-1.5-flash-latest";
    

    public ConverterConfiguration()
    {
    }

    public ConverterConfiguration(string? key)
    {
        Key = key;
    }

    public ConverterConfiguration(string? key, string model)
    {
        Key = key;
        Model = model;
    }

    public ConverterConfiguration(string? key, JsonSerializerOptions? jsonSerializerOptions, string model)
    {
        Key = key;
        JsonSerializerOptions = jsonSerializerOptions;
        Model = model;
    }
}