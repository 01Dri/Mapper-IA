
namespace MapperIA.Core.Interfaces;

public interface IClassMapper
{
    Task<TK> Map<T, TK>(T origin)
        where T : class
        where TK : class, new();

}