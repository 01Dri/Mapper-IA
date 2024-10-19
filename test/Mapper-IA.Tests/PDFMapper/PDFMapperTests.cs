using System.Text.Json.Serialization;
using ConvertersIA.Converters.Configuration;
using ConvertersIA.Interfaces;
using Extractors;
using MappersIA.PDFMapper;
using MappersIA.PDFMapper.Interfaces;

namespace Mapper_IA.Tests;

public class PDFMapperTests
{
    private readonly IPDFMapper _pdfMapper;

    public PDFMapperTests()
    {
        IAOptions iaOptions = new IAOptions()
        {
            Key = Environment.GetEnvironmentVariable("GEMINI_KEY")
        };
        IConverterIA geminiConverter = new GeminiConverter(iaOptions);
        _pdfMapper = new PDFMapper(geminiConverter, new PDFExtractor());
    }


    [Fact]
    public async Task Test_PDF_Converter_Should_Have_Map_CourseName_WithGeminiConverter()
    {
        var pdfPath = Path.Combine(@"../../../PDFMapper/PDFs/Curriculo - Diego.pdf");
        CurriculumModel curriculumModel =  await _pdfMapper.Map<CurriculumModel>(pdfPath);
        Assert.Equal("Uninter", curriculumModel.Faculdade);
        Assert.Equal("Análise e desenvolvimento de sistemas EAD", curriculumModel.Curso);

    }
}


public class CurriculumModel
{
    public string Faculdade { get; set; }
    public string Curso { get; set; }

}