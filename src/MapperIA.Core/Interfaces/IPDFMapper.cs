

namespace MapperIA.Core.Interfaces;

public interface IPDFMapper
{
    Task<T> Map<T>(string pdfPath) where T : class, new();


}