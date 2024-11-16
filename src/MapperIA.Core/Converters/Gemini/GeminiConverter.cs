using System.Text;
using System.Text.Json;
using MapperIA.Core.Configuration;
using MapperIA.Core.Converters.Prompts;
using MapperIA.Core.Exceptions;
using MapperIA.Core.Interfaces;
using MapperIA.Core.Models;
using MapperIA.Core.Models.Gemini.Request;
using MapperIA.Core.Models.Gemini.Response;

namespace MapperIA.Core.Converters.Gemini;

public class GeminiConverter : BaseConverters, IConverterIA
{
    public GeminiConverter(ConverterConfiguration converterConfiguration) : base(converterConfiguration)
    {
        
    }
    
    

    public async Task<T> SendPrompt<T>(string content, T objDestiny) where T : class
    {
        string objDestinyJson = JsonSerializer.Serialize(objDestiny, this.ConverterConfiguration.JsonSerializerOptions);
        BaseModelJson baseModelJson = EntityInitializer.InitializeBaseModel<T>(objDestinyJson);
        GeminiPromptRequest promptRequest = this.ProcessPromptRequest(content, false, baseModelJson, null);
        string promptRequestJson = JsonSerializer.Serialize(promptRequest);
        var mediaTypeRequest = new StringContent(promptRequestJson, Encoding.UTF8, "application/json");
        
        try
        {
            HttpResponseMessage response = await this.HttpClient.PostAsync(
                $"https://generativelanguage.googleapis.com/v1beta" +
                $"/models/{this.ConverterConfiguration.Model}:generateContent?key={this.ConverterConfiguration.Key}",
                mediaTypeRequest);

                if (!response.IsSuccessStatusCode)
                {
                    throw new RequestStatusIAException($"Request failed with status: {response.StatusCode}");
                }

                string responseData = await response.Content.ReadAsStringAsync();
                GeminiPromptResponse? responseObject = JsonSerializer.Deserialize<GeminiPromptResponse>(responseData, this.ConverterConfiguration.JsonSerializerOptions);
                if (responseObject == null)
                {
                    throw new FailedToSerializeException("Failed to serialize GeminiPromptResponse");
                }

                T? objSource = JsonSerializer.Deserialize<T>(this.ParseJsonResponse(responseObject),
                        this.ConverterConfiguration.JsonSerializerOptions);
                    EntityInitializer.CopyEntityProperties(objSource, objDestiny);
                    return objDestiny ?? throw new ConverterIAException("Unable to convert the destination object.");
        }
        catch (Exception ex)
        {
            throw new ConverterIAException($"An error occurred during processing: {ex.Message}");
        }
    }
    

    public async Task<string> SendPromptFileClassMapper(string content, FileClassMapperConfiguration configuration)
    {
        GeminiPromptRequest promptRequest = this.ProcessPromptRequest(content, true, null, configuration);
        string promptRequestJson = JsonSerializer.Serialize(promptRequest);
        var mediaTypeRequest = new StringContent(promptRequestJson, Encoding.UTF8, "application/json");

        try
        {
                HttpResponseMessage response = await this.HttpClient.PostAsync(
                    $"https://generativelanguage.googleapis.com/v1beta" +
                    $"/models/{this.ConverterConfiguration.Model}:generateContent?key={this.ConverterConfiguration.Key}",
                    mediaTypeRequest);

                if (!response.IsSuccessStatusCode)
                {
                    throw new RequestStatusIAException($"Request failed with status: {response.StatusCode}");

                }

                string responseData = await response.Content.ReadAsStringAsync();
                GeminiPromptResponse? responseObject = JsonSerializer.Deserialize<GeminiPromptResponse>(responseData, this.ConverterConfiguration.JsonSerializerOptions);
                if (responseObject == null)
                {
                    throw new FailedToSerializeException("Failed to serialize GeminiPromptResponse");
                }

                return responseObject
                        .Candidates.FirstOrDefault()
                        ?.Content.Parts.FirstOrDefault()
                        ?.Text ?? throw new ConverterIAException("Unable to convert the destination object.");
        }
        catch (Exception ex)
        {
            throw new ConverterIAException($"An error occurred during processing: {ex.Message}");
        }
    }

    private GeminiPromptRequest ProcessPromptRequest(
        string content,
        bool isFileClassMapper,
        BaseModelJson? baseModelJson,
        FileClassMapperConfiguration? configuration)
    {
        
        return  this.CreatePromptRequest
        (
            baseModelJson,
            content,
            isFileClassMapper,
            configuration?.NameSpaceValue,
            configuration?.NewClassFileName
        );
        
    }

    private GeminiPromptRequest CreatePromptRequest
    (
        BaseModelJson? baseModelJson,
        string content,
        bool isFileClassMapper,
        string? namespaceValue,
        string? newClassFileName
    )
    {
        PromptFacade prompt = new PromptFacade(baseModelJson, content);
        if (isFileClassMapper)
        {
            prompt.NameSpaceValue = namespaceValue;
            prompt.NewClassFileName = newClassFileName;
        }
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
                            Text = prompt.CreatePrompt(isFileClassMapper) 
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

        if (string.IsNullOrEmpty(text))
        {
            throw new ResponseIAException("The IA response does not contain any text.");
        }

        var cleanedJson = text
                .Replace("```json", string.Empty)
                .Replace("```", string.Empty)
                .Replace("\n", string.Empty)
                .Trim();
            return cleanedJson;
        }
    
}