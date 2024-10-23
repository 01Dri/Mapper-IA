
namespace MapperIA.Core.Interfaces;

public interface IClassMapper
{
    Task<T> Map<TK, T>(TK origin)
        where T : class, new () 
        where TK : class, new();

}