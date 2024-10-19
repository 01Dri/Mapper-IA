using System.Text.Json;
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
    public async Task Test_PDF_Converter_Should_To_Map_CourseName_And_Projects_On_CV_With_GeminiConverter()
    {
        var pdfPath = Path.Combine(@"../../../PDFMapper/PDFs/Curriculo - Diego.pdf");
        CurriculumModel curriculumModel =  await _pdfMapper.Map<CurriculumModel>(pdfPath);
        Assert.Equal("Uninter", curriculumModel.Faculdade);
        Assert.Equal("Análise e desenvolvimento de sistemas EAD", curriculumModel.Curso);
        Assert.Equal(2, curriculumModel.Projects.Count);
        
        var expectedProjectNames = new List<string> { "ReclameTrancoso", "VTHoftalon" };
        var actualProjectNames = curriculumModel.Projects.Select(p => p.Nome).ToList();

        Assert.Equal(expectedProjectNames, actualProjectNames);

    }

    public class CurriculumModel
    {
        public string Faculdade { get; set; }
        public string Curso { get; set; }
        public List<CurriculumProjects> Projects { get; set; }

    }


    public class CurriculumProjects
    {
        public string Nome { get; set; }
        public List<string> Tecnologias { get; set; } = new List<string>();
    }
}