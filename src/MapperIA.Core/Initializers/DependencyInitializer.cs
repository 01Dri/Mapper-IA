using System.Reflection;
using MapperIA.Core.Initializers.Dependencies;

namespace MapperIA.Core.Initializers;

public class DependencyInitializer
{
    private object _obj;

    public DependencyInitializer(object obj)
    {
        this._obj = obj;
    }

    public void Initialize()
    {
        var properties = _obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.PropertyType.IsGenericType)
            {
                var type = property.PropertyType.GetGenericTypeDefinition();

                switch (type)
                {
                    case var t when t == typeof(IEnumerable<>):
                        new EnumerableInitializer().InitializeDependencyProperties(this._obj);
                        break;
                    case var t when t == typeof(ICollection<>):
                        new CollectionInitializer().InitializeDependencyProperties(this._obj);
                        break;
                    case var t when t == typeof(List<>):
                        new ListInitializer().InitializeDependencyProperties(this._obj);
                        break;
                    default:
                        throw new ArgumentException("Not any initializer implementation for this property");
                }
            }
        }
    }
}