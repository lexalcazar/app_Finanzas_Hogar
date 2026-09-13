using System.ComponentModel.DataAnnotations;
using app_Fh_back.Models;

namespace app_Fh_back.Dtos.Auth;

public class RegisterDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}
