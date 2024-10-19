using ConvertersIA.Interfaces;
using Extractors.Interfaces;
using MappersIA.PDFMapper.Interfaces;

namespace MappersIA.PDFMapper;

public class PDFMapper : IPDFMapper
{
    private readonly IConverterIA _iaConverter;
    private readonly IPDFExtractor _pdfExtractor;
    private const int MAX_CONTENT_LENGTH = 10000;

    public PDFMapper(IConverterIA iaConverter, IPDFExtractor pdfExtractor)
    {
        _iaConverter = iaConverter;
        _pdfExtractor = pdfExtractor;
    }


    public async Task<T> Map<T>(string pdfPath) where T : class, new()
    {
        string content = _pdfExtractor.ExtractContent(pdfPath);
        if (content.Length > MAX_CONTENT_LENGTH)
        {
            content = content.Substring(0, MAX_CONTENT_LENGTH);
        }
        T result = await _iaConverter.SendPrompt<T>(content);
        return result;
    }
}