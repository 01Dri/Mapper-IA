using System.Text.Json;
using MapperIA.Core.Configuration;
using MapperIA.Core.Interfaces;

namespace MapperIA.Core.Mappers.PDFMapper;

public class PDFMapper : IPDFMapper
{
    private readonly IConverterIA _converterIa;
    private readonly IPDFExtractor _pdfExtractor;
    private const int MAX_CONTENT_LENGTH = 10000;

    public PDFMapper(IConverterIA converterIa, IPDFExtractor pdfExtractor)
    {
        _converterIa = converterIa;
        _pdfExtractor = pdfExtractor;
    }


    public async Task<T> Map<T>(string pdfPath) where T : class, new()
    {
        T result = new T();
        EntityUtils.InitializeDependencyProperties(result);
        string pdfContent = _pdfExtractor.ExtractContent(pdfPath);
        if (pdfContent.Length > MAX_CONTENT_LENGTH)
        {
            pdfContent = pdfContent.Substring(0, MAX_CONTENT_LENGTH);
        }

        string contentJson = JsonSerializer.Serialize(pdfContent);
        await _converterIa.SendPrompt(contentJson, result);
        return result;
    }
}