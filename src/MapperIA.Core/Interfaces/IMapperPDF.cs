namespace MapperIA.Core.Interfaces;

public interface IMapperPDF
{
    Task<T> Map<T>(string pdfPath) 
        where T : class, new();

    
}