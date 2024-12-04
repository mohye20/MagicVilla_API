namespace MagicVilla_Web.Models.Dto;

public class LoginRequestDTO
{
    public UserDTO User { get; set; }
    public string Password { get; set; }
}