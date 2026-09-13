using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using app_Fh_back.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace app_Fh_back.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<Usuario> _userManager;

    public TokenService(
        IConfiguration configuration,
        UserManager<Usuario> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<string> CrearTokenAsync(Usuario usuario)
    {
        var roles = await _userManager.GetRolesAsync(usuario);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id),
            new Claim(ClaimTypes.Email, usuario.Email ?? ""),
            new Claim(ClaimTypes.Name, usuario.Nombre)
        };

        foreach (var rol in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, rol));
        }

        var key = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT Key no configurada");

        var securityKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var credentials =
            new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}