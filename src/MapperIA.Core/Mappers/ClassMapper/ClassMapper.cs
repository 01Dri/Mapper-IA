using System.Text.Json;
using MapperIA.Core.Configuration;
using MapperIA.Core.Interfaces;

namespace MapperIA.Core.Mappers.ClassMapper;

public class ClassMapper : IClassMapper
{
    private readonly IConverterIA _converterIa;

    public ClassMapper(IConverterIA converterIa)
    {
        _converterIa = converterIa;
    }


    public async Task<TK> Map<T, TK>(T origin) where T : class where TK : class, new()
    {
        TK result = new TK();
        EntityInitializer.Initialize(result);
        string originJson = JsonSerializer.Serialize(origin);
        result = await _converterIa.SendPrompt(originJson, result);
        return result;
    }

}


