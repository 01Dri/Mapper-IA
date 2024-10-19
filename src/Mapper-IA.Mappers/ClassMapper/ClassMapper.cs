﻿using System.Text.Json;
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

    public async Task<T> Map<TK, T>(TK origin) 
        where TK : class, new() 
        where T : class, new()
    {
        T result = await _converterIa.SendPrompt<T>(JsonSerializer.Serialize(origin));
        return result;
    }
}