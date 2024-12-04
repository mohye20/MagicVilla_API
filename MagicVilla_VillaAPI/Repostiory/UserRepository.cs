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
        throw new NotImplementedException();
    }

    public Task<LoginRequestDTO> Login(LoginRequestDTO loginRequestDTO)
    {
        throw new NotImplementedException();
    }

    public Task<LocalUser> Register(RegistertionRequestDTO registertionRequestDTO)
    {
        throw new NotImplementedException();
    }
}