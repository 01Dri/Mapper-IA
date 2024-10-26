
namespace MapperIA.Core.Interfaces;

public interface IConverterIA
{
    Task<T> SendPrompt<T>(string content, T objDestiny, bool isFileClassMapper) where T : class;
    Task<string> SendPrompt(string content);


}