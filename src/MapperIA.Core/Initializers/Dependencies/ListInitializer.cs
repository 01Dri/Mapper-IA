using System.Collections;
using System.Reflection;
using MapperIA.Core.Exceptions;
using MapperIA.Core.Interfaces;

namespace MapperIA.Core.Initializers.Dependencies;

public class ListInitializer : IDependencyInitializer
{
    public void InitializeDependencyProperties<T>(T obj) where T : class
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.PropertyType.IsGenericType)
            {
                var itemType = property.PropertyType.GetGenericArguments()[0];
                var listInstance = Activator.CreateInstance(property.PropertyType);
                if (listInstance is not IList)
                {
                    throw new FailedToInitializeDependenciesException("Instance of attribute not implements 'IList'");
                }

                if (listInstance is IList<string> stringList)
                {
                    stringList.Add("");
                }
                else if (listInstance is IList<object> objectList)
                {

                    var itemInstance = Activator.CreateInstance(itemType);
                    if (itemInstance != null) objectList.Add(itemInstance);
                }

                property.SetValue(obj, listInstance);

            }
        }
    }
}