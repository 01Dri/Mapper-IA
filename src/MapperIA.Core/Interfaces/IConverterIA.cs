
using MapperIA.Core.Configuration;

namespace MapperIA.Core.Interfaces;

public interface IConverterIA
{
    Task<T> SendPrompt<T>(string content, T objDestiny) where T : class;
    Task<string> SendPromptFileClassMapper(string content, FileClassMapperOptions options);


}