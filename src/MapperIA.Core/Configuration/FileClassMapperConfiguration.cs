using MapperIA.Core.Helpers;

namespace MapperIA.Core.Configuration;

public class FileClassMapperConfiguration
{
    public string ClassFileName { get; set; }
    public string OutputFolder { get; set; } = FoldersHelpers.GetSolutionDefaultPath();
    public string? NewClassFileName { get; set; }
    public string? InputFolder { get; set; } = "Class";
    public string? NameSpaceValue { get; set; }
    

    public FileClassMapperConfiguration(string classFileName)
    {
        ClassFileName = classFileName;
        this.ValidateClassFileName();

    }

    public FileClassMapperConfiguration(string classFileName, string outputFolder)
    {
        this.ClassFileName = classFileName;
        this.OutputFolder = outputFolder;
        this.ValidateClassFileName();
    }
    private void ValidateClassFileName()
    {
        if (string.IsNullOrEmpty(ClassFileName))
            throw new ArgumentException("ClassFileName can't be null or empty");
    }

}

