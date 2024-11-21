using System.Reflection;
using MapperIA.Core.Exceptions;
using MapperIA.Core.Interfaces;

namespace MapperIA.Core.Initializers.Dependencies;

public class EnumerableInitializer : IDependencyInitializer
{
    public void InitializeDependencyProperties<T>(T obj) where T : class
    {
        var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.PropertyType.IsGenericType)
            {
                var itemType = property.PropertyType.GetGenericArguments()[0];

                var listInstance = Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));

                if (listInstance is not IEnumerable<object>)
                {
                    throw new FailedToInitializeDependenciesException(
                        "Instance of attribute does not implement 'ICollection<object>'");
                }

                if (listInstance is IEnumerable<string> enumerableList)
                {
                    var enumerableConvertedToList = enumerableList.ToList();
                    enumerableConvertedToList.Add("");
                    listInstance = enumerableConvertedToList.ToList();
                }
                else if (listInstance is IEnumerable<object> objectList)
                {
                    var enumerableConvertedToList = objectList.ToList();
                    var itemInstance = Activator.CreateInstance(itemType);
                    if (itemInstance != null)
                    {
                        enumerableConvertedToList.Add(itemType);
                        listInstance = enumerableConvertedToList.ToList();
                    }
                }
                property.SetValue(obj, listInstance);
            }
        }

    }
}