namespace MapperIA.Core.Helpers;

public class FoldersHelpers
{
    public static string GetSolutionDefaultPath()
    {
        string defaultSolutionPath = AppDomain.CurrentDomain.BaseDirectory;
        return Directory.GetParent(defaultSolutionPath).Parent.Parent.Parent.FullName;
    }

    public static string GetSolutionName()
    {
        string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        return new DirectoryInfo(solutionDirectory).Name;
    }
}