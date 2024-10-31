using MapperIA.Core.Configuration;
using MapperIA.Core.Converters.Gemini;
using MapperIA.Core.Extractors;
using MapperIA.Core.Helpers;
using MapperIA.Core.Interfaces;

namespace MapperIA.Tests.Mappers.FileClassMapper;

public class FileClassMapperTests
{
    private readonly IFileClassMapper _fileClassMapper;
    private const string OUTPUT_FOLDER = "MappedClasses";

    public  FileClassMapperTests()
    {
        ConverterOptions converterOptions = new ConverterOptions(Environment.GetEnvironmentVariable("GEMINI_KEY"));
        IConverterIA geminiConverter = new GeminiConverter(converterOptions);
        IExtractor fileClassExtractor = new ClassExtractor(); 
        _fileClassMapper = new MapperIA.Core.Mappers.ClassMapper.FileClassMapper(fileClassExtractor, geminiConverter);
    }

    
    [Fact]
    public async Task Test_Should_Create_NewClassFile_StudentCs_By_StudentPy()
    {
        FileClassMapperOptions options = new FileClassMapperOptions("Student.py", OUTPUT_FOLDER);
        string fullDirectoryResultPath = this.GetDirectoryResultPath();
        string fullFileResultPath = this.GetFileResultPath("Student.cs");
        
        await _fileClassMapper.Map(options);
        
        Assert.True(Directory.Exists(fullDirectoryResultPath));
        Assert.True(File.Exists(fullFileResultPath));

    }
    
    [Fact]
    public async Task Test_Should_Create_NewClassFile_CarroCs_By_CarroJava()
    {
     
        FileClassMapperOptions options = new FileClassMapperOptions("Carro.java", OUTPUT_FOLDER);

        string fullDirectoryResultPath = this.GetDirectoryResultPath();
        string fullFileResultPath = this.GetFileResultPath("Carro.cs");
        
        await _fileClassMapper.Map(options);
        
        Assert.True(Directory.Exists(fullDirectoryResultPath));
        Assert.True(File.Exists(fullFileResultPath));

    }
    
    [Fact]
    public async Task Test_Should_Create_NewClassFile_ContaBancariaRenomeadaCs_By_ContaBancariaJs()
    {
     
        FileClassMapperOptions options = new FileClassMapperOptions("ContaBancaria.js", OUTPUT_FOLDER);
        options.NewClassFileName = "ContaBancariaRenomeada";

        string fullDirectoryResultPath = this.GetDirectoryResultPath();
        string fullFileResultPath = this.GetFileResultPath("ContaBancariaRenomeada.cs");
        
        await _fileClassMapper.Map(options);
        
        Assert.True(Directory.Exists(fullDirectoryResultPath));
        Assert.True(File.Exists(fullFileResultPath));

    }
    
    [Fact]
    public async Task Test_Should_Create_NewClassFile_ContaBancariaCs_By_ContaBancariaJs()
    {
     
        FileClassMapperOptions options = new FileClassMapperOptions("ContaBancaria.js", OUTPUT_FOLDER);

        string fullDirectoryResultPath = this.GetDirectoryResultPath();
        string fullFileResultPath = this.GetFileResultPath("ContaBancaria.cs");
        
        await _fileClassMapper.Map(options);
        
        Assert.True(Directory.Exists(fullDirectoryResultPath));
        Assert.True(File.Exists(fullFileResultPath));

    }

    
    
    
    [Fact]
    public async Task Test_Should_Create_NewClassFile_UserCs_By_User_C_PLUS_PLUS()
    {
     
        // If output folder don't be sent, the mapper will to create the new file in default solution path
        FileClassMapperOptions options = new FileClassMapperOptions("Usuario.c++");

        string fullFileResultPath = Path.Combine(FoldersHelpers.GetSolutionDefaultPath(), "Usuario.cs");
        
        await _fileClassMapper.Map(options);
        
        Assert.True(File.Exists(fullFileResultPath));

    }
    
    [Fact]
    public async Task Test_Should_Create_NewClassFile_UserCs_By_User_C_PLUS_PLUS_In_Other_InputFolder()
    {
     
        FileClassMapperOptions options = new FileClassMapperOptions("Usuario.c++")
        {
            InputFolder = "Class_C_PLUS_PLUS",
            OutputFolder = "Mapped_C_Plus_Plus"
        };

        string fullFileResultPath = Path.Combine(FoldersHelpers.GetSolutionDefaultPath(),"Mapped_C_Plus_Plus" , "Usuario.cs");
        
        await _fileClassMapper.Map(options);
        
        Assert.True(File.Exists(fullFileResultPath));

    }

    
    
    [Fact]
    public void Test_Should_ThrowArgumentException_FileClassName()
    {
        var exception = Assert.Throws<ArgumentException>(() => new FileClassMapperOptions("", OUTPUT_FOLDER));
        Assert.Equal("ClassFileName can't be null", exception.Message);

    }



    private string GetDirectoryResultPath()
    {
        string solutionPath = FoldersHelpers.GetSolutionDefaultPath();
        return Path.Combine(solutionPath, OUTPUT_FOLDER);
    }
    
    private string GetFileResultPath(string classResultName)
    {
        string solutionPath = FoldersHelpers.GetSolutionDefaultPath();
        return  Path.Combine(solutionPath, OUTPUT_FOLDER, classResultName);
    }


}