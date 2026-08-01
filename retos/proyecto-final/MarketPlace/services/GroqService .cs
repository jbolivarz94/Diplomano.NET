using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

public class GroqService : IGroqService
{
    private readonly HttpClient _httpClient;
    private readonly GroqSettings _settings;

    public GroqService(
        HttpClient httpClient,
        IOptions<GroqSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;

        _httpClient.BaseAddress =
            new Uri(_settings.BaseUrl);

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
    }

    public async Task<string> AskAsync(string prompt)
    {
        var request = new
        {
            model = _settings.Model,
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = "Eres un asistente experto de AgroMarket Local que recomienda productos agrícolas frescos de agricultores locales."
                },
                new
                {
                    role = "user",
                    content = prompt
                }
            },
            temperature = 0.2
        };

        var json = JsonSerializer.Serialize(request);

        var response = await _httpClient.PostAsync(
            "chat/completions",
            new StringContent(json, Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        var groq = JsonSerializer.Deserialize<GroqResponse>(result,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return groq?.Choices.FirstOrDefault()?.Message.Content ?? "";
    }
}