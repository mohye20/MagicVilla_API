using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repostiory.IRepostiory;
using Microsoft.IdentityModel.Tokens;

namespace MagicVilla_VillaAPI.Repostiory;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;
    private string secretKey;

    public UserRepository(ApplicationDbContext db, IConfiguration configuration)
    {
        _db = db;
        secretKey = configuration.GetValue<string>("ApiSettings:Secret");
    }

    public bool IsUnique(string username)
    {
        var user = _db.LocalUsers.FirstOrDefault(x => x.Username == username);
        return user is null ? true : false;
    }

    public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
    {
        var user = _db.LocalUsers.FirstOrDefault(u =>
            u.Username.ToLower() == loginRequestDTO.UserName.ToLower() && u.Password == loginRequestDTO.Password);

        if (user is null)
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };


        var token = tokenHandler.CreateToken(tokenDescriptor);
        LoginResponseDTO loginResponseDto = new LoginResponseDTO()
        {
            Token = tokenHandler.WriteToken(token),
            User = user
        };

        return loginResponseDto;
    }

    public async Task<LocalUser> Register(RegistertionRequestDTO registertionRequestDTO)
    {
        LocalUser user = new()
        {
            Username = registertionRequestDTO.UserName,
            Name = registertionRequestDTO.Name,
            Role = registertionRequestDTO.Role,
            Password = registertionRequestDTO.Password
        };
        await _db.LocalUsers.AddAsync(user);
        await _db.SaveChangesAsync();
        user.Password = "";
        return user;
    }
}