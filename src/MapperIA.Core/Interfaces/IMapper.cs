namespace MapperIA.Core.Interfaces;

public interface IMapper
{
    Task<TK> Map<T, TK>(T origin)
        where T : class
        where TK : class, new();
    

}