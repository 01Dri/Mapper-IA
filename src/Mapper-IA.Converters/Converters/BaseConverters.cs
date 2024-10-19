using System.Text.Json;
using System.Text.Json.Serialization;
using ConvertersIA.Converters.Configuration;
using ConvertersIA.Exceptions;

namespace ConvertersIA.Converters;

public abstract class BaseConverters
{
    protected readonly IAOptions Options;
    protected readonly HttpClient HttpClient;

    protected BaseConverters(IAOptions iaOptions)
    {
        Options = iaOptions;
        if (string.IsNullOrEmpty(Options.Key)) throw new IAKeyException("API KEY Inválida!");
        if (iaOptions.jsonSerializerOptions == null)
        {
            iaOptions.jsonSerializerOptions = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.Never, //Nunca vai ignorar qualquer propriedade não prsente no objeto
                                                                    //(Retornando null se necessário)
                WriteIndented = true // Identação do JSON
            };
        }
        HttpClient = new HttpClient();
    }
}