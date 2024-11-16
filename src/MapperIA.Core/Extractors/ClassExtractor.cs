using MapperIA.Core.Interfaces;

namespace MapperIA.Core.Extractors;

public class ClassExtractor : IExtractor
{
    
    public string ExtractContent(string path)
    {
        if (string.IsNullOrEmpty(path)) throw new ArgumentException("The class path cannot be null or empty.");
        string classContent = File.ReadAllText(path);
        return classContent;
    }
}