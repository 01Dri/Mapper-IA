using ConvertersIA.Converters.Configuration;
using ConvertersIA.Interfaces;
using Extractors;
using MappersIA.PDFMapper.Interfaces;
 
namespace Mapper_IA.Tests.Mappers.PDFMapper;

public class PDFMapperTests
{
    private readonly IPDFMapper _pdfMapper;

    public PDFMapperTests()
    {
        OptionsIA optionsIa = new OptionsIA()
        {
            Key = Environment.GetEnvironmentVariable("GEMINI_KEY")
        };
        IConverterIA geminiConverter = new GeminiConverter(optionsIa);
        _pdfMapper = new MappersIA.PDFMapper.PDFMapper(geminiConverter, new PDFExtractor());
    }


    [Fact]
    public async Task Test_PDF_Converter_Should_Map_PDF_To_Model_GeminiConverter()
    {
        var pdfPath = Path.Combine(@"../../../Mappers/PDFMapper/PDFs/Curriculo - Diego.pdf");
        CurriculumModel curriculumModel =  await _pdfMapper.Map<CurriculumModel>(pdfPath);
        Assert.Equal("Uninter", curriculumModel.Faculdade);
        Assert.Equal("Análise e desenvolvimento de sistemas", curriculumModel.Curso);
        Assert.Equal(2, curriculumModel.Projects.Count);
        Assert.Equal("diegomagalhaesdev@gmail.com", curriculumModel.Email);
        
        var expectedProjectNames = new List<string> { "ReclameTrancoso", "VTHoftalon" };
        var actualProjectNames = curriculumModel.Projects.Select(p => p.Nome).ToList();

        Assert.Equal(expectedProjectNames, actualProjectNames);

    }

    public class CurriculumModel
    {
        public string Faculdade { get; set; }
        public string Curso { get; set; }
        public string Email { get; set; }
        
        public List<CurriculumProjects> Projects { get; set; }

    }


    public class CurriculumProjects
    {
        public string Nome { get; set; }
        public List<string> Tecnologias { get; set; } = new List<string>();
    }
}