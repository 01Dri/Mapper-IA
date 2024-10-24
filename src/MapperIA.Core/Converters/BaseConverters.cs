using System.Text.Json;
using System.Text.Json.Serialization;
using MapperIA.Core.Configuration;
using MapperIA.Core.Exceptions;

namespace MapperIA.Core.Converters;

public abstract class BaseConverters
{
    protected readonly OptionsIA Options;
    protected readonly HttpClient HttpClient;

    protected BaseConverters(OptionsIA optionsIa)
    {
        Options = optionsIa;
        
        if (string.IsNullOrEmpty(Options.Key)) throw new ApiKeyException("Invalid API Key!");
        if (optionsIa.JsonSerializerOptions == null)
        {
            optionsIa.JsonSerializerOptions = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.Never, 
                WriteIndented = true
            };
        }

        if (string.IsNullOrEmpty(optionsIa.Model)) optionsIa.Model = "gemini-1.5-flash-latest";
        HttpClient = new HttpClient();
    }
}