using System.Net.Http.Json;
using app_Fh_back.Configuration;
using app_Fh_back.Dtos.IA;
using Microsoft.Extensions.Options;

namespace app_Fh_back.Services.IA;

public class LmStudioService : ILmStudioService
{
    private readonly HttpClient _httpClient;
    private readonly LmStudioOptions _options;

    public LmStudioService(
        HttpClient httpClient,
        IOptions<LmStudioOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<ChatResponseDto> EnviarMensajeAsync(string mensaje)
    {
        var request = new
        {
            model = _options.Model,
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = mensaje
                }
            }
        };

        var response = await _httpClient.PostAsJsonAsync(
            $"{_options.BaseUrl}/v1/chat/completions",
            request
        );

        response.EnsureSuccessStatusCode();

        var resultado = await response.Content
            .ReadFromJsonAsync<LmStudioResponse>();

        var contenido = resultado?
            .Choices
            .FirstOrDefault()?
            .Message
            .Content;

        return new ChatResponseDto
        {
            Respuesta = contenido ?? "El modelo no ha devuelto ninguna respuesta."
        };
    }

    private class LmStudioResponse
    {
        public List<Choice> Choices { get; set; } = [];
    }

    private class Choice
    {
        public Message Message { get; set; } = new();
    }

    private class Message
    {
        public string Content { get; set; } = string.Empty;
    }

    public async Task<string> EnviarPromptAsync(
        string systemPrompt,
        string mensaje)
    {
        var request = new
        {
            model = _options.Model,
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = systemPrompt
                },
                new
                {
                    role = "user",
                    content = mensaje
                }
            
            },
            temperature = 0,
            max_tokens = 200
        };

        var response = await _httpClient.PostAsJsonAsync(
            $"{_options.BaseUrl}/v1/chat/completions",
            request
        );

        response.EnsureSuccessStatusCode();

        var resultado = await response.Content
            .ReadFromJsonAsync<LmStudioResponse>();

        return resultado?
            .Choices
            .FirstOrDefault()?
            .Message
            .Content
            ?? string.Empty;
    }
}