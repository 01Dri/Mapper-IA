
namespace MapperIA.Core.Interfaces;

public interface IDependencyInitializer
{
    void InitializeDependencyProperties<T>(T obj) where T : class;
}