using System.Text.Json;

namespace MapperIA.Core.Configuration;

public class OptionsIA
{
    public string? Key { get; set; }
    public JsonSerializerOptions? JsonSerializerOptions { get; set; }
    public string Model { get; set; }
    

    public OptionsIA()
    {
    }

    public OptionsIA(string? key)
    {
        Key = key;
    }

    public OptionsIA(string? key, string model)
    {
        Key = key;
        Model = model;
    }

    public OptionsIA(string? key, JsonSerializerOptions? jsonSerializerOptions, string model)
    {
        Key = key;
        JsonSerializerOptions = jsonSerializerOptions;
        Model = model;
    }
}