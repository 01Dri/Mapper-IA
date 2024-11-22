using System.Reflection;
using MapperIA.Core.Interfaces;

namespace MapperIA.Core.Initializers;

public class DependencyInitializerFacade
{
    private object _obj;
    private readonly IDependencyInitializer _dependencyInitializer;

    public DependencyInitializerFacade(object obj, IDependencyInitializer dependencyInitializer)
    {
        _obj = obj;
        _dependencyInitializer = dependencyInitializer;
    }


    public void Initialize()
    {
        var properties = _obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var genericProperties = properties.Where(x => x.PropertyType.IsGenericType);
        foreach (var property in genericProperties)
        {
            foreach (var itemType in property.PropertyType.GetGenericArguments())
            {
                _dependencyInitializer.InitializeDependencyProperties(itemType, property, _obj);
            }
        }
    }
}