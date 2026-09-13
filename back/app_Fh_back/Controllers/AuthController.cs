using Microsoft.AspNetCore.Mvc;
using app_Fh_back.Dtos.Auth;
using Microsoft.AspNetCore.Identity;
using app_Fh_back.Services;
using app_Fh_back.Models;
namespace app_Fh_back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // Inyectar UserManager y SignInManager
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly TokenService _tokenService;
    // Constructor
    public AuthController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, TokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var usuario = new Usuario
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            Nombre = registerDto.Nombre
        };

        var result = await _userManager.CreateAsync(usuario, registerDto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        // Asignar el rol "User" al nuevo usuario
        var roleResult = await _userManager.AddToRoleAsync(usuario, "User");
        // si falla
        if (!roleResult.Succeeded)
        {
            return BadRequest(roleResult.Errors);
        }

        return Ok(new { message = "Usuario registrado exitosamente" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var usuario =
            await _userManager.FindByEmailAsync(loginDto.Email);

        if (usuario == null)
        {
            return Unauthorized(new
            {
                message = "Email o contraseña incorrectos"
            });
        }

        var resultado =
            await _signInManager.CheckPasswordSignInAsync(
                usuario,
                loginDto.Password,
                false);

        if (!resultado.Succeeded)
        {
            return Unauthorized(new
            {
                message = "Email o contraseña incorrectos"
            });
        }

        var token =
            await _tokenService.CrearTokenAsync(usuario);

        return Ok(new
        {
            token
        });
    }
}