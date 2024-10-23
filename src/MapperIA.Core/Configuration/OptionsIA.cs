using System.Text.Json;

namespace MapperIA.Core.Configuration;

public class OptionsIA
{
    public string? Key { get; set; }
    public JsonSerializerOptions? jsonSerializerOptions { get; set; }

    public OptionsIA()
    {
    }

    public OptionsIA(string? key)
    {
        Key = key;
    }

    public OptionsIA(string? key, JsonSerializerOptions? jsonSerializerOptions)
    {
        Key = key;
        this.jsonSerializerOptions = jsonSerializerOptions;
    }
}