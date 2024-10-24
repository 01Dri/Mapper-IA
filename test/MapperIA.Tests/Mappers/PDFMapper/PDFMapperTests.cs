using MapperIA.Core.Configuration;
using MapperIA.Core.Converters.Gemini;
using MapperIA.Core.Extractors;
using MapperIA.Core.Interfaces;

namespace Mapper_IA.Tests.Mappers.PDFMapper;

public class PDFMapperTests
{
    
    

    [Fact]
    public async Task Test_PDF_Converter_Should_Map_PDF_To_Model_GeminiConverter_FlashModel()
    {
        OptionsIA optionsIa = new OptionsIA(Environment.GetEnvironmentVariable("GEMINI_KEY"), "gemini-1.5-flash");
        IConverterIA geminiConverter = new GeminiConverter(optionsIa);
        IPDFMapper pdfMapper = new MapperIA.Core.Mappers.PDFMapper.PDFMapper(geminiConverter, new PDFExtractor());

        var pdfPath = Path.Combine(@"../../../Mappers/PDFMapper/PDFs/Curriculo - Diego.pdf");
        CurriculumModel curriculumModel =  await pdfMapper.Map<CurriculumModel>(pdfPath);
        Assert.Equal("Uninter", curriculumModel.Faculdade);
        Assert.Equal("Análise e desenvolvimento de sistemas", curriculumModel.Curso);
        Assert.Equal(2, curriculumModel.Projects.Count);
        Assert.Equal("diegomagalhaesdev@gmail.com", curriculumModel.Email);
        
        var expectedProjectNames = new List<string> { "ReclameTrancoso", "VTHoftalon" };
        var actualProjectNames = curriculumModel.Projects.Select(p => p.Nome).ToList();

        Assert.Equal(expectedProjectNames, actualProjectNames);

    }
    
    [Fact]
    public async Task Test_PDF_Converter_Should_Map_PDF_To_Model_GeminiConverter_ProModel()
    {
        OptionsIA optionsIa = new OptionsIA(Environment.GetEnvironmentVariable("GEMINI_KEY"), "gemini-1.5-pro");
        IConverterIA geminiConverter = new GeminiConverter(optionsIa);
        IPDFMapper pdfMapper = new MapperIA.Core.Mappers.PDFMapper.PDFMapper(geminiConverter, new PDFExtractor());

        var pdfPath = Path.Combine(@"../../../Mappers/PDFMapper/PDFs/Curriculo - Diego.pdf");
        CurriculumModel curriculumModel =  await pdfMapper.Map<CurriculumModel>(pdfPath);
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