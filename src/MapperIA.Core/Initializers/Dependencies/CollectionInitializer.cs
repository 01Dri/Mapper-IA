using System.Collections;
using System.Reflection;
using MapperIA.Core.Exceptions;
using MapperIA.Core.Interfaces;

namespace MapperIA.Core.Initializers.Dependencies;

public class CollectionInitializer : IDependencyInitializer
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
            
                if (listInstance is not ICollection)
                {
                    throw new FailedToInitializeDependenciesException(
                        "Instance of attribute does not implement 'ICollection'");
                }   

                if (listInstance is ICollection<string> stringList)
                {
                    stringList.Add("");
                }
                else if (listInstance is ICollection<object> objectList)
                {
                    var itemInstance = Activator.CreateInstance(itemType);
                    if (itemInstance != null)
                    {
                        objectList.Add(itemInstance);
                    }
                }

                property.SetValue(obj, listInstance);
            }
        }
    }
}