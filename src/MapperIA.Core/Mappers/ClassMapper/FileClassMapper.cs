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
        string classFileContentJson = this.GetClassFileContentJson(classFileName);
        string resultContentByPrompt = await _converterIa.SendPrompt(classFileContentJson);
        resultContentByPrompt = this.CleanResultContent(resultContentByPrompt);

        string outputFolderPath = Path.Combine(defaultSolutionPath, outputFolder);
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

    private string GetClassFileContentJson(string classFileName)
    {
        string defaultSolutionPath = AppDomain.CurrentDomain.BaseDirectory;
        string defaultPath = Directory.GetParent(defaultSolutionPath).Parent.Parent.Parent.FullName;
        string fullPath = Path.Combine(defaultPath, "Class", classFileName);
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
}