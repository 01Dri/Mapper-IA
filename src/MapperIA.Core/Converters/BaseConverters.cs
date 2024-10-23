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
        
        if (string.IsNullOrEmpty(Options.Key)) throw new IAKeyException("API KEY Inválida!");
        if (optionsIa.jsonSerializerOptions == null)
        {
            optionsIa.jsonSerializerOptions = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.Never, //Nunca vai ignorar qualquer propriedade não prsente no objeto
                //(Retornando null se necessário)
                WriteIndented = true // Identação do JSON
            };
        }
        HttpClient = new HttpClient();
    }
}