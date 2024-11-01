using System.Text;
using System.Text.Json;
using MapperIA.Core.Configuration;
using MapperIA.Core.Exceptions;
using MapperIA.Core.Interfaces;
using MapperIA.Core.Models;
using MapperIA.Core.Models.Gemini.Request;
using MapperIA.Core.Models.Gemini.Response;

namespace MapperIA.Core.Converters.Gemini;

public class GeminiConverter : BaseConverters, IConverterIA
{
    public GeminiConverter(ConverterOptions converterOptions) : base(converterOptions)
    {
    }

    public async Task<T> SendPrompt<T>(string content, T objDestiny) where T : class
    {
        string objDestinyJson = JsonSerializer.Serialize(objDestiny, this.ConverterOptions.JsonSerializerOptions);
        BaseModelJson baseModelJson = EntityInitializer.InitializeBaseModel<T>(objDestinyJson);
        GeminiPromptRequest promptRequest = this.CreatePromptRequest(baseModelJson, content, false, null, null);
        string promptRequestJson = JsonSerializer.Serialize(promptRequest);
        var mediaTypeRequest = new StringContent(promptRequestJson, Encoding.UTF8, "application/json");
        
        try
        {
            HttpResponseMessage response = await this.HttpClient.PostAsync(
                $"https://generativelanguage.googleapis.com/v1beta" +
                $"/models/{this.ConverterOptions.Model}:generateContent?key={this.ConverterOptions.Key}",
                mediaTypeRequest);
            
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                GeminiPromptResponse? responseObject = JsonSerializer.Deserialize<GeminiPromptResponse>(responseData, this.ConverterOptions.JsonSerializerOptions);
                if (responseObject != null)
                {
                    T? objSource = JsonSerializer.Deserialize<T>(this.ParseJsonResponse(responseObject), this.ConverterOptions.JsonSerializerOptions);
                    EntityInitializer.CopyEntityProperties(objSource, objDestiny);
                    return objDestiny ?? throw new ConverterIAException("Unable to convert the destination object.");
                }

                throw new FailedToSerializeException("Failed to serialize GeminiPromptResponse");
            }
            throw new RequestStatusIAException($"Request failed with status: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            throw new ConverterIAException($"An error occurred during processing: {ex.Message}");
        }
    }

    public async Task<string> SendPromptFileClassMapper(string content, FileClassMapperOptions options)
    {
        GeminiPromptRequest promptRequest = this.CreatePromptRequest
            (null, content, true, options.NameSpaceValue, options.NewClassFileName);
        string promptRequestJson = JsonSerializer.Serialize(promptRequest);
        var mediaTypeRequest = new StringContent(promptRequestJson, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await this.HttpClient.PostAsync(
                $"https://generativelanguage.googleapis.com/v1beta" +
                $"/models/{this.ConverterOptions.Model}:generateContent?key={this.ConverterOptions.Key}",
                mediaTypeRequest);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                GeminiPromptResponse? responseObject = JsonSerializer.Deserialize<GeminiPromptResponse>(responseData, this.ConverterOptions.JsonSerializerOptions);
                if (responseObject != null)
                {
                    return responseObject
                        .Candidates.FirstOrDefault()
                        ?.Content.Parts.FirstOrDefault()
                        ?.Text ?? throw new ConverterIAException("Unable to convert the destination object.");
                }
                throw new FailedToSerializeException("Failed to serialize GeminiPromptResponse");

            }
            
            throw new RequestStatusIAException($"Request failed with status: {response.StatusCode}");

        }
        catch (Exception ex)
        {
            throw new ConverterIAException($"An error occurred during processing: {ex.Message}");
        }
    }

    private GeminiPromptRequest CreatePromptRequest(
        BaseModelJson? baseModelJson, string content,
        bool isFileClassMapper, string? namespaceValue, string? newClassFileName)
    {
        return new GeminiPromptRequest()
        {
            Contents = new List<Contents>
            {
                new Contents()
                {
                    Parts = new List<Parts>
                    {
                        new Parts()
                        {
                            Text = !isFileClassMapper 
                                ? DefaultMapperPrompt(baseModelJson, content)
                                : FileClassConverterPrompt(content, namespaceValue, newClassFileName)
                        }
                    }
                }
            }
        };
        
    }

    private string ParseJsonResponse(GeminiPromptResponse? response)
    {
        if (response == null) 
            throw new ResponseIAException("The IA response is null.");

        var candidate = response.Candidates.FirstOrDefault();
        var part = candidate?.Content.Parts.FirstOrDefault();
        var text = part?.Text;
        
        if (!string.IsNullOrEmpty(text))
        {
            var cleanedJson = text
                .Replace("```json", string.Empty)
                .Replace("```", string.Empty)
                .Replace("\n", string.Empty)
                .Trim();
            return cleanedJson;
        }

        throw new ResponseIAException("The IA response does not contain any text.");
    }

    private string DefaultMapperPrompt(BaseModelJson baseModelJson, string content)
    {
        return $"Please return a JSON that strictly follows the structure: {baseModelJson.BaseJson}. \n" +
               $"1. This JSON should be filled with the following values: {content}. \n" +
               $"2. If you need to know which attributes are required to fill the JSON, you can check here: {JsonSerializer.Serialize(baseModelJson.Types)}. \n" +
               $"3. If there are any college-related information, please specify the type of college (EAD or in-person), as applicable. \n" +
               $"4. The returned JSON will be used for deserialization, so ensure it is properly formatted for conversion into an object. \n" +
               $"5. The structure and data must adhere to the model. \n" +
               $"6. If any value in the content does not match the structure, do not include that value and return 'null' instead. \n" +
               $"7. Additionally, provide only the JSON directly, without any comments or explanations.";
    }

    private string FileClassConverterPrompt(string content, string? namespaceValue, string? newClassFileName)
    {
        
        return
            $"Please convert the following code into a C# class: \n" +
            $"The original code is as follows: {content}. \n" +
            $"Follow these conversion rules: \n" +
            $"1. Use C# naming conventions (PascalCase for classes and properties). \n" +
            $"2. Replace any language-specific types with equivalent C# types where applicable (e.g., int, string). \n" +
            $"3. Ensure methods, properties, and constructors are converted to valid C# syntax. \n" +
            $"4. Add necessary using directives at the top of the C# file. \n" +
            $"5. Ensure that the returned C# class is formatted correctly and ready for compilation. \n" +
            $"6. If the namespace value is not null, set the C# class with the following namespace declaration: 'namespace {namespaceValue};' (without braces). If the namespace value is null, omit the namespace declaration entirely. \n" +
            $"7. Define properties using auto-implemented syntax (e.g., 'public string Name {{ get; set; }}'). \n" +
            $"8. If a property is auto-implemented, do not include any corresponding methods for getting or checking values. \n" +
            $"9. Ensure that the new class uses the same language as the original code (e.g., if the original code is in Portuguese, the new class should also be in Portuguese). \n" +
            $"10. If the new class name variable is not null, change the class name to the value sent: {newClassFileName}. \n" +
            $"11. Provide only the converted C# class code without any comments or explanations. \n";
    }
}