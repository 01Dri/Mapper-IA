using System.Collections;
using System.Reflection;

namespace MapperIA.Core.Configuration;

public abstract class EntityInitializer
{
    
    public static void Initialize<T>(T obj) where T : class
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.PropertyType.IsGenericType && 
                property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var itemType = property.PropertyType.GetGenericArguments()[0];
                var listInstance = (IList)Activator.CreateInstance(property.PropertyType);
                var itemInstance = Activator.CreateInstance(itemType);
                listInstance.Add(itemInstance);
                property.SetValue(obj, listInstance);
            }
        }
    }
}