using MapperIA.Core.Configuration;
using MapperIA.Core.Converters.Gemini;
using MapperIA.Core.Extractors;
using MapperIA.Core.Interfaces;

namespace MapperIA.Tests.Mappers.FileClassMapper;

public class FileClassMapperTests
{
    private readonly IFileClassMapper _fileClassMapper;

    public  FileClassMapperTests()
    {
        OptionsIA optionsIa = new OptionsIA(Environment.GetEnvironmentVariable("GEMINI_KEY"));
        IConverterIA geminiConverter = new GeminiConverter(optionsIa);
        IExtractor fileClassExtractor = new ClassExtractor(); 
        _fileClassMapper = new MapperIA.Core.Mappers.ClassMapper.FileClassMapper(fileClassExtractor, geminiConverter);
    }

    
    // ACHAR UMA FORMA DE CRIAR O NAMESPACE NA CLASSE
    [Fact]
    public async Task Test()
    {
        await _fileClassMapper.Map("Test.java", "MappedClasses");
        await _fileClassMapper.Map("Carro.java", "MappedClasses");

    }

}