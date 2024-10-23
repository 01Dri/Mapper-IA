
namespace MapperIA.Core.Interfaces;

public interface IConverterIA
{
    Task<T> SendPrompt<T>(string content) where T : class, new();

}