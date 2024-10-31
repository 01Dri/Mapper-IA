namespace MapperIA.Core.Interfaces;

public interface IFileClassMapper
{
    Task<string> Map
    (
        string classFileName,
        string outputFolder,
        string? newClassFileName=null
    );

}