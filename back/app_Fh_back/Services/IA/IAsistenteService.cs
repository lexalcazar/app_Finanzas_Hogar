using app_Fh_back.Dtos.IA;

namespace app_Fh_back.Services.IA;

public interface IAsistenteService
{
    Task<ChatResponseDto> ProcesarMensajeAsync(
        string usuarioId,
        string mensaje
    );
}