using MapperIA.Core.Interfaces;

namespace MapperIA.Core.Extractors;

public class ClassExtractor : IExtractor
{
    
    public string ExtractContent(string path)
    {
        string classContent = File.ReadAllText(path);
        return classContent;
    }
}