using System.Text.RegularExpressions;
using MapperIA.Core.Interfaces;

namespace MapperIA.Core.Mappers.ClassMapper;

public class FileClassMapper : IFileClassMapper
{
    private readonly IExtractor _extractor;
    private readonly IConverterIA _converterIa;
    
    public FileClassMapper(IExtractor extractor, IConverterIA converterIa)
    {
        _extractor = extractor;
        _converterIa = converterIa;
    }

    public async Task<string> Map(
        
        string classFileName,
        string outputFolder,
        string? newClassNameResult = null)
    {

        string defaultSolutionPath = this.GetSolutionDefaultPath();
        string classFileContentJson = this.GetClassFileContent(classFileName);
        string outputFolderPath = Path.Combine(defaultSolutionPath, outputFolder);
        
        string resultContentByPrompt = await _converterIa.SendPrompt(
            classFileContentJson, this.GetNamespaceValue(outputFolder));
        
        resultContentByPrompt = this.CleanResultContent(resultContentByPrompt);

        string fullOutputFolder =
            Path.Combine(outputFolderPath,
                $"{this.GetClassFileNameResult
                (newClassNameResult,
                    resultContentByPrompt)}.cs");


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
        string fullPath = Path.Combine(this.GetSolutionDefaultPath(), "Class", classFileName);
        return  _extractor.ExtractContent(fullPath);
    }

    private string GetSolutionDefaultPath()
    {
        string defaultSolutionPath = AppDomain.CurrentDomain.BaseDirectory;
        return Directory.GetParent(defaultSolutionPath).Parent.Parent.Parent.FullName;
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
    private  string GetSolutionName()
    {
        string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        return new DirectoryInfo(solutionDirectory).Name;
    }

    private string GetNamespaceValue(string outputFolder)
    {
        return Path.Combine(this.GetSolutionName(), outputFolder);
    }
    
}