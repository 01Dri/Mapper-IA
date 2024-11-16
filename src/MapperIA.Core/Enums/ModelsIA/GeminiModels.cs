using System.ComponentModel;

namespace MapperIA.Core.Enums.ModelsIA;

public enum GeminiModels
{
    // Other gemini models here.
    [Description("gemini-1.5-flash-latest")]
    FLASH_1_5_LATEST,
    [Description("gemini-1.5-pro")]
    FLASH_1_5_PRO 
}

public static class EnumsExtensions
{
    public static string GetValue(this Enum enumValue)
    {
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
        if (fieldInfo == null) throw new Exception("Failed to get fields info from enum");
        var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (attributes.Length == 0) throw new Exception("Failed to get attributes from enum");
        return attributes[0].Description;
    }
}