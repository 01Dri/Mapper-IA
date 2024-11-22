using System.Reflection;
using MapperIA.Core.Interfaces;

namespace MapperIA.Core.Initializers;

public class DependencyInitializer : IDependencyInitializer
{
    public void InitializeDependencyProperties(Type? itemType, PropertyInfo property, object obj)
    {
        if (itemType == null)
            throw new ArgumentNullException(nameof(itemType));

        var listType = typeof(List<>).MakeGenericType(itemType);
        var listInstance = Activator.CreateInstance(listType);

        var addMethod = listType.GetMethod("Add");
        if (addMethod == null)
            throw new InvalidOperationException("Not found 'Add' method in Generic List!");

        object? itemInstance;

        if (itemType == typeof(string))
        {
            itemInstance = "";
        }
        else
        {
            itemInstance = Activator.CreateInstance(itemType);
        }

        if (itemInstance != null)
        {
            addMethod.Invoke(listInstance, new[] { itemInstance });
        }

        property.SetValue(obj, listInstance);
    }

}