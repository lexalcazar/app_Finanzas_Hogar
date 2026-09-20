using System.Security.Claims;
using app_Fh_back.Dtos.IA;
using app_Fh_back.Services.IA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app_Fh_back.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AsistenteController : ControllerBase
{
    private readonly IAsistenteService _asistenteService;

    public AsistenteController(IAsistenteService asistenteService)
    {
        _asistenteService = asistenteService;
    }

    [HttpPost("chat")]
    public async Task<ActionResult<ChatResponseDto>> Chat(
        [FromBody] ChatRequestDto request)
    {
        var usuarioId =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(usuarioId))
        {
            return Unauthorized();
        }

        if (string.IsNullOrWhiteSpace(request.Mensaje))
        {
            return BadRequest(new
            {
                message = "El mensaje no puede estar vacío"
            });
        }

        var respuesta =
            await _asistenteService.ProcesarMensajeAsync(
                usuarioId,
                request.Mensaje);

        return Ok(respuesta);
    }
}