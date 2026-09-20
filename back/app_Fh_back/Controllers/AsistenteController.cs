using app_Finanzas_Hogar.Dtos.IA;
using app_Finanzas_Hogar.Services.IA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app_Finanzas_Hogar.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AsistenteController : ControllerBase
{
    private readonly ILmStudioService _lmStudioService;

    public AsistenteController(ILmStudioService lmStudioService)
    {
        _lmStudioService = lmStudioService;
    }

    [HttpPost("chat")]
    public async Task<ActionResult<ChatResponseDto>> Chat(
        [FromBody] ChatRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Mensaje))
        {
            return BadRequest("El mensaje no puede estar vacío.");
        }

        var respuesta = await _lmStudioService
            .EnviarMensajeAsync(request.Mensaje);

        return Ok(respuesta);
    }
}