using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using MapperIA.Core.Configuration;
using MapperIA.Core.Helpers;
using MapperIA.Core.Interfaces;

namespace MapperIA.Core.Mappers.ClassMapper;

public class FileClassMapper : IFileClassMapper
{
    private readonly IExtractor _extractor;
    private readonly IConverterIA _converterIa;
    private readonly string SolutionFolderPath;
    private readonly string SolutionName;
    
    public FileClassMapper(IExtractor extractor, IConverterIA converterIa)
    {
        _extractor = extractor;
        _converterIa = converterIa;
        SolutionFolderPath = FoldersHelpers.GetProjectDefaultPath();
        SolutionName = FoldersHelpers.GetProjectName();
    }

    public async Task<string> Map(FileClassMapperConfiguration configuration)
    {
        string classFileContentJson = this.GetClassFileContent(configuration.ClassFileName, configuration.InputFolder);
        string outputFolderPath = Path.Combine(SolutionFolderPath, configuration.OutputFolder);
        
        configuration.NameSpaceValue = this.GetNamespaceValue(configuration.ProjectName,configuration.OutputFolder);
        configuration.NewClassFileName =
            this.GetClassFileNameResult(configuration.NewClassFileName, classFileContentJson);
        
        string resultContentByPrompt = await _converterIa.SendPromptFileClassMapper(classFileContentJson, configuration);
        
        resultContentByPrompt = this.CleanResultContent(resultContentByPrompt);
        string fullOutputFolder = this.GetFullOutputFolder(outputFolderPath, configuration.NewClassFileName);

        if (!Directory.Exists(outputFolderPath))
        {
            Directory.CreateDirectory(outputFolderPath);
        }

        File.WriteAllText(fullOutputFolder, resultContentByPrompt);

        return resultContentByPrompt;
    }

    private string GetClassFileNameResult(string? newClassNameResult, string content)
    {

        if (string.IsNullOrEmpty(newClassNameResult))
        {
            string pattern = @"\bclass\s+(\w+)(?:\s*{|\s*:|$)";
            Match match = Regex.Match(content, pattern);
            if (match.Success)
            {
                return  match.Groups[1].Value;
            }
        }

        return newClassNameResult;
    }

    private string GetClassFileContent(string classFileName, string? inputFolder)
    {
        if (string.IsNullOrEmpty(inputFolder)) inputFolder = "Class";
        string fullPath = Path.Combine(SolutionFolderPath, inputFolder, classFileName);
        return  _extractor.ExtractContent(fullPath);
    }


    private string CleanResultContent(string result)
    {
        return result.Replace("'''", string.Empty)
            .Replace("csharp", string.Empty)
            .Replace("```", string.Empty)
            .Replace("\r\n", "\n")
            .Replace("\n\n", "\n")
            .Replace("\n", Environment.NewLine)
            .Trim();
    }

    private string? GetNamespaceValue(string solutionName, string outputFolder)
    {
        return Path.Combine(solutionName, outputFolder);

    }

    private string GetFullOutputFolder(string outputFolderPath, string newClassNameResult)
    {
        return Path.Combine(outputFolderPath, $"{newClassNameResult}.cs");
    }
    
    
}