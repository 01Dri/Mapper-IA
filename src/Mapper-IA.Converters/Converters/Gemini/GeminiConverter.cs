using System.Text;
using System.Text.Json;
using ConvertersIA.Converters;
using ConvertersIA.Converters.Configuration;
using ConvertersIA.Exceptions;
using ConvertersIA.Interfaces;
using ConvertersIA.Models.Gemini.Request;
using ConvertersIA.Models.Gemini.Response;
using Mapper_IA.EntitiesSetup;

public class GeminiConverter : BaseConverters, IConverterIA
{
    public GeminiConverter(IAOptions iaOptions) : base(iaOptions)
    {
    }
    public async Task<T> SendPrompt<T>(string content) where T : class, new()
    {
        T? obj = new T();
        EntityInitializer.Initialize(obj);
        var contentJson = GetContentJson(content);
        var objJson = JsonSerializer.Serialize(obj, this.Options.jsonSerializerOptions);
        var promptRequest = this.CreatePromptRequest(objJson, contentJson);
        var promptRequestJson = JsonSerializer.Serialize(promptRequest);
        var mediaTypeRequest = new StringContent(promptRequestJson, Encoding.UTF8, "application/json");
        
        try
        {
            var response = await this.HttpClient.PostAsync(
                $"https://generativelanguage.googleapis.com/v1beta" +
                $"/models/gemini-1.5-flash-latest:generateContent?key={this.Options.Key}",
                mediaTypeRequest);
            
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<GeminiPromptResponse>(responseData, this.Options.jsonSerializerOptions);
                obj = JsonSerializer.Deserialize<T>(this.ParseJsonResponseIA(responseObject), this.Options.jsonSerializerOptions);
                return obj ?? throw new ConverterException("Não foi possível converter.");
            }
            throw new IARequestStatusException($"Ocorreu uma falha na requisição: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            throw new ConverterException("Ocorreu uma falha, causa: " + ex.Message);
        }
    }


    private GeminiPromptRequest CreatePromptRequest(string objJson, string contentJson)
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
                            Text =
                            $"Por favor, retorne um JSON que siga rigorosamente a estrutura a seguir: {objJson}. " +
                            $"Esse JSON deve ser preenchido com os seguintes valores: {contentJson}. " +
                            $"O JSON retornado será utilizado em uma operação de deserialização, portanto, assegure-se de que ele está formatado corretamente para ser transformado em um objeto. " +
                            $"A estrutura e os dados devem estar em conformidade com os requisitos especificados no modelo. Sendo os nomes dos atributos em inglês ou não." +
                            $"Se algum valor presente no conteúdo não estiver de acordo com a estrutura, não adicione esse valor à resposta e retorne 'null' em seu lugar. " +
                            $"Além disso, não inclua comentários ou explicações na sua resposta; forneça apenas o JSON diretamente."
                        }
                    }
                }
            }
        };
    }

    private string ParseJsonResponseIA(GeminiPromptResponse? response)
    {
        if (response == null) throw new IAResponseException("IA Response é nulo.");
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

        throw new IAResponseException("IA Response não possui texto.");
    }
    
    private string GetContentJson(string jsonString)
    {
        try
        {
            JsonDocument.Parse(jsonString);
            return jsonString; 
        }
        catch (JsonException)
        {
            return JsonSerializer.Serialize(jsonString, this.Options.jsonSerializerOptions);
        }
    }

}