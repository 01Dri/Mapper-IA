using System.Text.Json.Serialization;

namespace ConvertersIA.Models.Gemini.Response;

public class GeminiPromptResponse
{
    [JsonPropertyName("candidates")]
    public List<CandidatesResponse> Candidates { get; set; }
}