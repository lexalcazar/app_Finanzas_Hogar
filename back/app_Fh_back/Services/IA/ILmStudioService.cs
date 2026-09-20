using app_Fh_back.Dtos.IA;

namespace app_Fh_back.Services.IA;

public interface ILmStudioService
{
    Task<ChatResponseDto> EnviarMensajeAsync(string mensaje);
    Task<string> EnviarPromptAsync(
    string systemPrompt,
    string mensaje);
}