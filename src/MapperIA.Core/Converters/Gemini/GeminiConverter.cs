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
    public async Task<T> SendPrompt<T>(string content, T? objDestiny) where T : class
    {
        var objJson = JsonSerializer.Serialize(objDestiny, this.Options.JsonSerializerOptions);
        var baseModelJson = EntityInitializer.InitializeBaseModel(objDestiny, objJson);
        var promptRequest = this.CreatePromptRequest(baseModelJson, content);
        var promptRequestJson = JsonSerializer.Serialize(promptRequest);
        var mediaTypeRequest = new StringContent(promptRequestJson, Encoding.UTF8, "application/json");
        
        try
        {
            var response = await this.HttpClient.PostAsync(
                $"https://generativelanguage.googleapis.com/v1beta" +
                $"/models/{this.Options.Model}:generateContent?key={this.Options.Key}",
                mediaTypeRequest);
            
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<GeminiPromptResponse>(responseData, this.Options.JsonSerializerOptions);
                objDestiny = JsonSerializer.Deserialize<T>(this.ParseJsonResponseIA(responseObject), this.Options.JsonSerializerOptions);
                return objDestiny ?? throw new ConverterException("Não foi possível converter.");
            }
            throw new IARequestStatusException($"Ocorreu uma falha na requisição: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            throw new ConverterException("Ocorreu uma falha, causa: " + ex.Message);
        }
    }


    private GeminiPromptRequest CreatePromptRequest(BaseModelJson baseModelJson, string content)
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
                                $"Por favor, retorne um JSON que siga rigorosamente a estrutura a seguir: {baseModelJson.BaseJson}. " +
                                $"Esse JSON deve ser preenchido com os seguintes valores: {content}. \n" +
                                $"Caso necessite saber, quais são os atributos necessários para preencher o JSON, você pode verificar aqui: {JsonSerializer.Serialize(baseModelJson.Types)}. " +
                                $"Se houver informações relacionadas a faculdades, especifique o tipo da faculdade (EAD ou presencial), conforme aplicável. " +
                                $"O JSON retornado será utilizado para deserialização, portanto, assegure-se de que ele esteja formatado corretamente para ser transformado em um objeto. " +
                                $"A estrutura e os dados devem seguir o modelo. " +
                                $"Se algum valor no conteúdo não corresponder à estrutura, não adicione esse valor e retorne 'null' em seu lugar. " +
                                $"Além disso, forneça apenas o JSON diretamente, sem incluir comentários ou explicações."
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

}