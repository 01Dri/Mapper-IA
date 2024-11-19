using System.Reflection;
using MapperIA.Core.Configuration;
using MapperIA.Core.Converters.Gemini;
using MapperIA.Core.Enums.ModelsIA;
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
        ConverterConfiguration converterConfiguration = new ConverterConfiguration(Environment.GetEnvironmentVariable("GEMINI_KEY"), GeminiModels.FLASH_1_5_PRO.GetValue());
        IConverterIA geminiConverter = new GeminiConverter(converterConfiguration);
        IExtractor fileClassExtractor = new ClassExtractor(); 
        _fileClassMapper = new MapperIA.Core.Mappers.ClassMapper.FileClassMapper(fileClassExtractor, geminiConverter);
    }

    
    [Fact]
    public async Task Test_Should_Create_NewClassFile_StudentCs_By_StudentPy()
    {
        FileClassMapperConfiguration configuration = new FileClassMapperConfiguration("Student.py", "MapperIA.Tests", OUTPUT_FOLDER);
        string fullDirectoryResultPath = this.GetDirectoryResultPath();
        string fullFileResultPath = this.GetFileResultPath("Student.cs");
        
        await _fileClassMapper.Map(configuration);
        
        Assert.True(Directory.Exists(fullDirectoryResultPath));
        Assert.True(File.Exists(fullFileResultPath));

    }
    
    [Fact]
    public async Task Test_Should_Create_NewClassFile_CarroCs_By_CarroJava()
    {
     
        FileClassMapperConfiguration configuration = new FileClassMapperConfiguration("Carro.java", "MapperIA.Tests", OUTPUT_FOLDER);

        string fullDirectoryResultPath = this.GetDirectoryResultPath();
        string fullFileResultPath = this.GetFileResultPath("Carro.cs");
        
        await _fileClassMapper.Map(configuration);
        
        Assert.True(Directory.Exists(fullDirectoryResultPath));
        Assert.True(File.Exists(fullFileResultPath));

    }
    
    [Fact]
    public async Task Test_Should_Create_NewClassFile_ContaBancariaRenomeadaCs_By_ContaBancariaJs()
    {
     
        FileClassMapperConfiguration configuration = new FileClassMapperConfiguration("ContaBancaria.js", "MapperIA.Tests", OUTPUT_FOLDER);
        configuration.NewClassFileName = "ContaBancariaRenomeada";

        string fullDirectoryResultPath = this.GetDirectoryResultPath();
        string fullFileResultPath = this.GetFileResultPath("ContaBancariaRenomeada.cs");
        
        await _fileClassMapper.Map(configuration);
        
        Assert.True(Directory.Exists(fullDirectoryResultPath));
        Assert.True(File.Exists(fullFileResultPath));

    }
    
    [Fact]
    public async Task Test_Should_Create_NewClassFile_ContaBancariaCs_By_ContaBancariaJs()
    {
     
        FileClassMapperConfiguration configuration = new FileClassMapperConfiguration("ContaBancaria.js", "MapperIA.Tests", OUTPUT_FOLDER);

        string fullDirectoryResultPath = this.GetDirectoryResultPath();
        string fullFileResultPath = this.GetFileResultPath("ContaBancaria.cs");
        
        await _fileClassMapper.Map(configuration);
        
        Assert.True(Directory.Exists(fullDirectoryResultPath));
        Assert.True(File.Exists(fullFileResultPath));

    }

    
    
    
    [Fact]
    public async Task Test_Should_Create_NewClassFile_UserCs_By_User_C_PLUS_PLUS()
    {
     
        // If output folder don't be sent, the mapper will to create the new file in default solution path
        FileClassMapperConfiguration configuration = new FileClassMapperConfiguration("Usuario.c++", Assembly.GetExecutingAssembly().GetName().Name);

        string fullFileResultPath = Path.Combine(FoldersHelpers.GetProjectDefaultPath(), "Usuario.cs");
        
        await _fileClassMapper.Map(configuration);
        
        Assert.True(File.Exists(fullFileResultPath));

    }
    
    [Fact]
    public async Task Test_Should_Create_NewClassFile_UserCs_By_User_C_PLUS_PLUS_In_Other_InputFolder()
    {
         
        FileClassMapperConfiguration configuration = new FileClassMapperConfiguration("Usuario.c++", "MapeadorIA")
        {
            InputFolder = "Class_C_PLUS_PLUS",
            OutputFolder = "Mapped_C_Plus_Plus",
        };

        string fullFileResultPath = Path.Combine(FoldersHelpers.GetProjectDefaultPath(),"Mapped_C_Plus_Plus" , "Usuario.cs");
        
        await _fileClassMapper.Map(configuration);
        
        Assert.True(File.Exists(fullFileResultPath));

    }

    
    
    [Fact]
    public void Test_Should_ThrowArgumentException_FileClassName()
    {
        var exception = Assert.Throws<ArgumentException>(() => new FileClassMapperConfiguration("", OUTPUT_FOLDER));
        Assert.Equal("ClassFileName can't be null or empty", exception.Message);

    }



    private string GetDirectoryResultPath()
    {
        string solutionPath = FoldersHelpers.GetProjectDefaultPath();
        return Path.Combine(solutionPath, OUTPUT_FOLDER);
    }
    
    private string GetFileResultPath(string classResultName)
    {
        string solutionPath = FoldersHelpers.GetProjectDefaultPath();
        return  Path.Combine(solutionPath, OUTPUT_FOLDER, classResultName);
    }


}