using System.Text.Json;
using System.Text.Json.Serialization;
using MapperIA.Core.Configuration;
using MapperIA.Core.Exceptions;

namespace MapperIA.Core.Converters;

public abstract class BaseConverters
{
    protected readonly ConverterOptions ConverterOptions;
    protected readonly HttpClient HttpClient;

    protected BaseConverters(ConverterOptions converterOptions)
    {
        ConverterOptions = converterOptions;
        
        if (string.IsNullOrEmpty(ConverterOptions.Key)) throw new ApiKeyException("Invalid API Key!");
        if (converterOptions.JsonSerializerOptions == null)
        {
            converterOptions.JsonSerializerOptions = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.Never, 
                WriteIndented = true
            };
        }

        HttpClient = new HttpClient();
    }
}