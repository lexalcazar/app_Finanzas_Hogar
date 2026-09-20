using app_Finanzas_Hogar.Dtos.IA;

namespace app_Finanzas_Hogar.Services.IA;

public interface ILmStudioService
{
    Task<ChatResponseDto> EnviarMensajeAsync(string mensaje);
    Task<string> EnviarPromptAsync(
    string systemPrompt,
    string mensaje);
}