using System.Text.Json;
using MapperIA.Core.Configuration;
using MapperIA.Core.Interfaces;

namespace MapperIA.Core.Mappers.PDFMapper;

public class PDFMapper : IMapperPDF
{
    private readonly IConverterIA _converterIa;
    private readonly IExtractor _pdfExtractor;
    private const int MAX_CONTENT_LENGTH = 10000;

    public PDFMapper(IConverterIA converterIa, IExtractor pdfExtractor)
    {
        _converterIa = converterIa;
        _pdfExtractor = pdfExtractor;
    }


    public async Task<T> Map<T>(string pdfPath) where T : class, new()
    {
        string pdfContent = _pdfExtractor.ExtractContent(pdfPath);
        
        T result = new T();
        EntityUtils.InitializeDependencyProperties(result);
        if (string.IsNullOrEmpty(pdfContent))
            throw new ArgumentException("The serialization of the pdf content resulted in invalid content.");
        
        if (pdfContent.Length > MAX_CONTENT_LENGTH)
        {
            pdfContent = pdfContent.Substring(0, MAX_CONTENT_LENGTH);
        }

        string contentJson = JsonSerializer.Serialize(pdfContent);
        await _converterIa.SendPrompt(contentJson, result);
        return result;
    }
}