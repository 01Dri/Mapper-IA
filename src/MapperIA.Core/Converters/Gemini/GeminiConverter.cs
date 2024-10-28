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
    public GeminiConverter(OptionsIA optionsIa) : base(optionsIa)
    {
    }

    public async Task<T> SendPrompt<T>(string content, T objDestiny) where T : class
    {
        string objDestinyJson = JsonSerializer.Serialize(objDestiny, this.Options.JsonSerializerOptions);
        BaseModelJson baseModelJson = EntityUtils.InitializeBaseModel<T>(objDestinyJson);
        GeminiPromptRequest promptRequest = this.CreatePromptRequest(baseModelJson, content, false, null);
        string promptRequestJson = JsonSerializer.Serialize(promptRequest);
        var mediaTypeRequest = new StringContent(promptRequestJson, Encoding.UTF8, "application/json");
        
        try
        {
            HttpResponseMessage response = await this.HttpClient.PostAsync(
                $"https://generativelanguage.googleapis.com/v1beta" +
                $"/models/{this.Options.Model}:generateContent?key={this.Options.Key}",
                mediaTypeRequest);
            
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                GeminiPromptResponse? responseObject = JsonSerializer.Deserialize<GeminiPromptResponse>(responseData, this.Options.JsonSerializerOptions);
                if (responseObject != null)
                {
                    T? objSource = JsonSerializer.Deserialize<T>(this.ParseJsonResponseIA(responseObject), this.Options.JsonSerializerOptions);
                    EntityUtils.CopyEntityProperties(objSource, objDestiny);
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

    public async Task<string> SendPrompt(string content, string namespaceValue)
    {
        GeminiPromptRequest promptRequest = this.CreatePromptRequest
            (null, content, true, namespaceValue);
        string promptRequestJson = JsonSerializer.Serialize(promptRequest);
        var mediaTypeRequest = new StringContent(promptRequestJson, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await this.HttpClient.PostAsync(
                $"https://generativelanguage.googleapis.com/v1beta" +
                $"/models/{this.Options.Model}:generateContent?key={this.Options.Key}",
                mediaTypeRequest);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                GeminiPromptResponse? responseObject = JsonSerializer.Deserialize<GeminiPromptResponse>(responseData, this.Options.JsonSerializerOptions);
                if (responseObject != null)
                {
                    return responseObject
                        .Candidates.FirstOrDefault()
                        ?.Content.Parts.FirstOrDefault()
                        ?.Text ?? throw new ConverterIAException("Unable to convert the destination object.");
                }
            }
            throw new FailedToSerializeException("Failed to serialize GeminiPromptResponse");

        }
        catch (Exception ex)
        {
            throw new ConverterIAException($"An error occurred during processing: {ex.Message}");
        }
    }

    private GeminiPromptRequest CreatePromptRequest(
        BaseModelJson? baseModelJson, string content,
        bool isFileClassMapper, string? namespaceValue)
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
                                : FileClassConverterPrompt(content, namespaceValue)
                        }
                    }
                }
            }
        };
        
    }

    private string ParseJsonResponseIA(GeminiPromptResponse? response)
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

    private string FileClassConverterPrompt(string content, string namespaceValue)
    {
        return
            $"Please convert the following Java class into a C# class. " +
            $"The Java class is as follows: {content}. \n" +
            $"Please ensure the conversion follows these rules: \n" +
            $"1. Use C# conventions for naming (PascalCase for classes and properties). \n" +
            $"2. Replace Java types with equivalent C# types (e.g., int -> int, String -> string). \n" +
            $"3. Ensure that methods and constructors are converted appropriately. \n" +
            $"4. Include any necessary using directives at the top of the C# file. \n" +
            $"5. The returned C# class should be properly formatted and ready for compilation. \n" +
            $"6. Ensure that the returned C# class has this namespace declaration: 'namespace {namespaceValue};' (with a semicolon at the end and without braces). \n" +
            $"7. Define properties using auto-implemented syntax, for example: 'public string Name {{ get; set; }}'. \n" +
            $"8. Do not include any comments or explanations; just return the C# class code. \n";
    }

}
