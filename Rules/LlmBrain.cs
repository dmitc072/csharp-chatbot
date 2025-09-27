using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

// LLM-backed brain: posts a chat request and returns the first reply text.
public class LlmBrain : IChatBrain
{
    private static readonly HttpClient _http = new HttpClient();                 // reuse client
    private readonly string _endpoint = Environment.GetEnvironmentVariable("LLM_ENDPOINT")
        ?? throw new InvalidOperationException("Missing LLM_ENDPOINT");
    private readonly string _apiKey = Environment.GetEnvironmentVariable("LLM_API_KEY")
        ?? throw new InvalidOperationException("Missing LLM_API_KEY");
    private readonly string _model = Environment.GetEnvironmentVariable("LLM_MODEL") ?? "gpt-4o-mini";

    public async Task<string> GetReplyAsync(string input)
    {
        try
        {
            // 1) Build chat-completions style body (system + user message)
            var payload = new
            {
                model = _model,
                messages = new[] {
                    new { role = "system", content = "You are a helpful C# console chatbot. Be concise." },
                    new { role = "user",   content = input }
                }
            };

            // 2) Serialize JSON and add auth header
            var json = JsonSerializer.Serialize(payload);
            using var req = new HttpRequestMessage(HttpMethod.Post, _endpoint)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

            // 3) Send, ensure success, parse minimal path to the text reply
            using var res = await _http.SendAsync(req);
            res.EnsureSuccessStatusCode();
            var body = await res.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(body);

            // Expect: { choices: [ { message: { content: "..." } } ] }
            return doc.RootElement.GetProperty("choices")[0]
                     .GetProperty("message").GetProperty("content").GetString()
                   ?? "(empty LLM reply)";
        }
        catch (Exception ex)
        {
            return $"(LLM error — {ex.GetType().Name}: {ex.Message})";
        }
    }
}
