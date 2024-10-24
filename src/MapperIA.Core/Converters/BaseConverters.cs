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
        if (optionsIa.JsonSerializerOptions == null)
        {
            optionsIa.JsonSerializerOptions = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.Never, //Nunca vai ignorar qualquer propriedade não prsente no objeto
                //(Retornando null se necessário)
                WriteIndented = true // Identação do JSON
            };
        }

        if (string.IsNullOrEmpty(optionsIa.Model)) optionsIa.Model = "gemini-1.5-flash-latest";
        HttpClient = new HttpClient();
    }
}