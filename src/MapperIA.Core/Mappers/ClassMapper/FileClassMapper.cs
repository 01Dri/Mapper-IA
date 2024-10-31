using System.Text.RegularExpressions;
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
        SolutionFolderPath = FoldersHelpers.GetSolutionDefaultPath();
        SolutionName = FoldersHelpers.GetSolutionName();
    }

    public async Task<string> Map
    (
        string classFileName,
        string outputFolder,
        string? newClassFileName = null
        
        )
    {
        string classFileContentJson = this.GetClassFileContent(classFileName);
        string outputFolderPath = Path.Combine(SolutionFolderPath, outputFolder);
        
        string resultContentByPrompt = await _converterIa.SendPrompt(classFileContentJson, this.GetNamespaceValue(outputFolder));
        
        resultContentByPrompt = this.CleanResultContent(resultContentByPrompt);
        string fullOutputFolder = this.GetFullOutputFolder(outputFolderPath, newClassFileName, resultContentByPrompt);

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
            string pattern = @"class\s+(\w+)\s*{";
            Match match = Regex.Match(content, pattern);
            if (match.Success)
            {
                return  match.Groups[1].Value;
            }
        }

        return newClassNameResult;
    }

    private string GetClassFileContent(string classFileName)
    {
        string fullPath = Path.Combine(SolutionFolderPath, "Class", classFileName);
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

    private string GetNamespaceValue(string outputFolder)
    {
        return Path.Combine(SolutionName, outputFolder);
    }

    private string GetFullOutputFolder(string outputFolderPath, string newClassNameResult, string resultContentByPrompt)
    {
        return Path.Combine(outputFolderPath, $"{this.GetClassFileNameResult
        (newClassNameResult,
            resultContentByPrompt)}.cs");
    }
    
    
}