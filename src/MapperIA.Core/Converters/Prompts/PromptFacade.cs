using System.Text.Json;
using MapperIA.Core.Models;

namespace MapperIA.Core.Converters.Prompts;

public class PromptFacade
{
    
    public BaseModelJson? BaseModelJson { get; set; }
    public string Content { get; set; }
    public string? NameSpaceValue { get; set; }
    public string? NewClassFileName { get; set; }

    public PromptFacade(BaseModelJson baseModelJson, string content)
    {
        BaseModelJson = baseModelJson;
        Content = content;
    }
    
    public string CreatePrompt(bool isFileClassMapper)
    {
        if (isFileClassMapper) return FileClassConverterPrompt();
        if (BaseModelJson == null) throw new ArgumentException("BaseModelJson can't be null!");
        var prompt = this.DefaultMapperPrompt();
        return DefaultMapperPrompt();
    }


    private string DefaultMapperPrompt()
    {
        return $"Please return a JSON that strictly follows the structure: {BaseModelJson?.BaseJson}. \n" +
               $"1. This JSON should be filled with the following values: {Content}. \n" +
               $"2. If you need to know which attributes are required to fill the JSON, you can check here: {JsonSerializer.Serialize(BaseModelJson?.Types)}. \n" +
               $"3. If there are any college-related information, please specify the type of college (EAD or in-person), as applicable. \n" +
               $"4. The returned JSON will be used for deserialization, so ensure it is properly formatted for conversion into an object. \n" +
               $"5. The structure and data must adhere to the model. \n" +
               $"6. If any value in the content does not match the structure, do not include that value and return 'null' instead. \n" +
               $"7. Additionally, provide only the JSON directly, without any comments or explanations.";
    }

    private string FileClassConverterPrompt()
    {
        return
            $"Please convert the following code into a C# class: \n" +
            $"The original code is as follows: {Content}. \n" +
            $"Follow these conversion rules: \n" +
            $"1. Use C# naming conventions (PascalCase for classes and properties). \n" +
            $"2. Replace any language-specific types with equivalent C# types where applicable (e.g., int, string). \n" +
            $"3. Ensure methods, properties, and constructors are converted to valid C# syntax. \n" +
            $"4. Add necessary using directives at the top of the C# file. \n" +
            $"5. Ensure that the returned C# class is formatted correctly and ready for compilation. \n" +
            $"6. If the namespace value is not null, set the C# class with the following namespace declaration: 'namespace value => {NameSpaceValue};' (without braces). Ensure the namespace is in the standard C# format, e.g., 'MapperIA.Tests'. If the namespace value is null, omit the namespace declaration entirely. \n" +
            $"7. Define properties using auto-implemented syntax (e.g., 'public string Name {{ get; set; }}'). \n" +
            $"8. If a property is auto-implemented, do not include any corresponding methods for getting or checking values. \n" +
            $"9. Ensure that the new class uses the same language as the original code (e.g., if the original code is in Portuguese, the new class should also be in Portuguese). \n" +
            $"10. NewClassFileName Value: {NewClassFileName}. If the new class file name value is not null, replace the class name in the original code with the value of the new class file name. \n" +
            $"11. Provide only the converted C# class code without any comments or explanations. \n";
    }

}