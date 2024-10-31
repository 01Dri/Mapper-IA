using System.Text.Json;

namespace MapperIA.Core.Configuration;

public class ConverterOptions
{
    public string? Key { get; set; }
    public JsonSerializerOptions? JsonSerializerOptions { get; set; }
    public string Model { get; set; }
    

    public ConverterOptions()
    {
    }

    public ConverterOptions(string? key)
    {
        Key = key;
    }

    public ConverterOptions(string? key, string model)
    {
        Key = key;
        Model = model;
    }

    public ConverterOptions(string? key, JsonSerializerOptions? jsonSerializerOptions, string model)
    {
        Key = key;
        JsonSerializerOptions = jsonSerializerOptions;
        Model = model;
    }
}