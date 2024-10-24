﻿using System.Collections;
using System.Reflection;
using MapperIA.Core.Models;

namespace MapperIA.Core.Configuration;

public static class EntityInitializer
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

    public static BaseModelJson InitializeBaseModel<T>(T obj, string objJson) 
    {
        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties();
        BaseModelJson baseModelJson = new BaseModelJson(objJson);

        foreach (var property in properties)
        {
            baseModelJson.Types.Add(new BaseTypes()
            {
                Name = property.Name,
                Type = property.PropertyType.ToString()
            });
        }

        return baseModelJson;
    }
    
}