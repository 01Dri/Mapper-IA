using MapperIA.Core.Helpers;

namespace MapperIA.Core.Configuration;

public class FileClassMapperOptions
{
    public string ClassFileName { get; set; }
    public string OutputFolder { get; set; } = FoldersHelpers.GetSolutionDefaultPath();
    public string? NewClassFileName { get; set; }
    public string? InputFolder { get; set; } = "Class";
    public string? NameSpaceValue { get; set; }
    

    public FileClassMapperOptions(string classFileName)
    {
        ClassFileName = classFileName;
        OutputFolder = FoldersHelpers.GetSolutionDefaultPath();
    }

    public FileClassMapperOptions(string classFileName, string outputFolder)
    {
        ClassFileName = classFileName;
        OutputFolder = outputFolder;
        if (string.IsNullOrEmpty(ClassFileName)) throw new ArgumentException("ClassFileName can't be null");
    }

}

