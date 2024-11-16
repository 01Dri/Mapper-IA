using MapperIA.Core.Configuration;
using MapperIA.Core.Converters.Gemini;
using MapperIA.Core.Enums.ModelsIA;
using MapperIA.Core.Extractors;
using MapperIA.Core.Interfaces;

namespace MapperIA.Tests.Mappers.PDFMapper;

public class PDFMapperTests
{
    private readonly ConverterConfiguration _converterConfiguration;
    private readonly IConverterIA _converterIa;
    public PDFMapperTests()
    {
        _converterConfiguration = new ConverterConfiguration
        (
            Environment.GetEnvironmentVariable("GEMINI_KEY"),
            GeminiModels.FLASH_1_5_PRO.GetValue()
        );
        _converterIa = new GeminiConverter(_converterConfiguration);
    }

    [Fact]
    public async Task Test_Should_Throw_FileNotFoundException_When_PdfFile_Does_Not_Exist()
    {
        IMapperPDF pdfMapper = new MapperIA.Core.Mappers.PDFMapper.PDFMapper(_converterIa, new PDFExtractor());

        string pdfPath = Path.Combine("path/to/nonexistent/file.pdf");

        var exception = await Assert.ThrowsAsync<FileNotFoundException>(
            async () => await pdfMapper.Map<CurriculumModel>(pdfPath)
        );

        Assert.Equal("The specified PDF file does not exist.", exception.Message);
        Assert.Equal(pdfPath, exception.FileName);
    }

    [Fact]
    public async Task Test_PDF_Converter_Should_Map_PDF_To_Model_GeminiConverter_FlashModel()
    {
        IMapperPDF pdfMapper = new MapperIA.Core.Mappers.PDFMapper.PDFMapper(_converterIa, new PDFExtractor());

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
        IMapperPDF pdfMapper = new MapperIA.Core.Mappers.PDFMapper.PDFMapper(_converterIa, new PDFExtractor());
        var pdfPath = Path.Combine(@"../../../Mappers/PDFMapper/PDFs/Curriculo - Diego.pdf");
        CurriculumModel curriculumModel =  await pdfMapper.Map<CurriculumModel>(pdfPath);
        Assert.Contains("Uninter", curriculumModel.Faculdade);
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