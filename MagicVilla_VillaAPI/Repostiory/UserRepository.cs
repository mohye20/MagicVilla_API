using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repostiory.IRepostiory;

namespace MagicVilla_VillaAPI.Repostiory;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public bool IsUnique(string username)
    {
        var user = _db.LocalUsers.FirstOrDefault(x => x.Username == username);
        return user is null ? true : false;
    }

    public Task<LoginRequestDTO> Login(LoginRequestDTO loginRequestDTO)
    {
        throw new NotImplementedException();
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