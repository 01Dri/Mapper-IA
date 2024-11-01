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
    }

    public FileClassMapperOptions(string classFileName, string outputFolder)
    {
        this.ClassFileName = classFileName;
        this.OutputFolder = outputFolder;
        if (string.IsNullOrEmpty(this.ClassFileName)) throw new ArgumentException("ClassFileName can't be null");
    }


}

