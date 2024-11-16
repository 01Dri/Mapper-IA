using System.Text.Json;
using System.Text.Json.Serialization;
using MapperIA.Core.Configuration;
using MapperIA.Core.Exceptions;

namespace MapperIA.Core.Converters;

public abstract class BaseConverters
{
    protected readonly ConverterConfiguration ConverterConfiguration;

    protected readonly HttpClient HttpClient;

    protected BaseConverters(ConverterConfiguration converterConfiguration)
    {
        ConverterConfiguration = converterConfiguration;
        
        if (string.IsNullOrEmpty(ConverterConfiguration.Key)) throw new ApiKeyException("Invalid API Key!");
        if (converterConfiguration.JsonSerializerOptions == null)
        {
            converterConfiguration.JsonSerializerOptions = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.Never, 
                WriteIndented = true
            };
        }

        HttpClient = new HttpClient();
    }
}