using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ConvertersIA.Converters.Configuration;
using ConvertersIA.Exceptions;
using ConvertersIA.Interfaces;
using ConvertersIA.Models.Gemini.Request;
using ConvertersIA.Models.Gemini.Response;

public class GeminiConverter : IConverterIA
{
    private readonly HttpClient _httpClient;
    private readonly IAOptions _iaOptions;

    public GeminiConverter(IAOptions iaOptions )
    {
        _httpClient = new HttpClient();
        _iaOptions = iaOptions;
        if (iaOptions.jsonSerializerOptions == null)
        {
            iaOptions.jsonSerializerOptions = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                WriteIndented = true
            };
        }
        if (string.IsNullOrEmpty(_iaOptions.Key)) throw new IAKeyException("API KEY Inválida!");
    }
    

    public async Task<T> SendPrompt<T>(string content) where T : class, new()
    {
        T obj = new T();
        var contentJson = JsonSerializer.Serialize(content, _iaOptions.jsonSerializerOptions);
        var objJson = JsonSerializer.Serialize(obj, _iaOptions.jsonSerializerOptions);
        var promptRequest = this.CreatePromptRequest(objJson, contentJson);
        var promptRequestJson = JsonSerializer.Serialize(promptRequest);
        var mediaTypeRequest = new StringContent(promptRequestJson, Encoding.UTF8, "application/json");
        
        try
        {
            var response = await _httpClient.PostAsync(
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent?key={this._iaOptions.Key}",
                mediaTypeRequest);
            
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<GeminiPromptResponse>(responseData, _iaOptions.jsonSerializerOptions);
                obj = JsonSerializer.Deserialize<T>(this.ParseJsonIAResponse(responseObject), _iaOptions.jsonSerializerOptions);
                return obj;
            }
            throw new IARequestStatusException($"Ocorreu uma falha na requisição: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            throw new Exception("Ocorreu uma falha, causa: " + ex.Message);
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
                            $"A estrutura e os dados devem estar em conformidade com os requisitos especificados no modelo. " +
                            $"Se algum valor presente no conteúdo não estiver de acordo com a estrutura, não adicione esse valor à resposta e retorne 'null' em seu lugar. " +
                            $"Além disso, não inclua comentários ou explicações na sua resposta; forneça apenas o JSON diretamente."
                        }
                    }
                }
            }
        };
    }

    private string ParseJsonIAResponse(GeminiPromptResponse? response)
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
}