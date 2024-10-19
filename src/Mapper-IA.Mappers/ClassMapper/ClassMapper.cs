using System.Text.Json;
using ConvertersIA.Interfaces;
using MappersIA.PDFMapper.Interfaces;

namespace Mappers.ClassMapper;

public class ClassMapper : IClassMapper
{
    private readonly IConverterIA _converterIa;

    public ClassMapper(IConverterIA converterIa)
    {
        _converterIa = converterIa;
    }

    // public async Task<T> Map<TK, T>(TK origin) where T : class, new() where TK : class, new();
    // {
    //     
    //     // Olhar se não estou serializando o origin duas vezes (uma aqui e a outra no converter)
    //     T result = await _converterIa.SendPrompt<T>(origin.ToString());
    //     return result;
    // }
    public async Task<T> Map<TK, T>(TK origin) where TK : class, new() where T : class, new()
    {
        T result = await _converterIa.SendPrompt<T>(JsonSerializer.Serialize(origin));
        return result;
    }
}