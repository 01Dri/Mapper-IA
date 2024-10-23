using System.Text.Json.Serialization;

namespace MapperIA.Core.Models.Gemini.Response;

public class GeminiPromptResponse
{
    [JsonPropertyName("candidates")]
    public List<CandidatesResponse> Candidates { get; set; }
}