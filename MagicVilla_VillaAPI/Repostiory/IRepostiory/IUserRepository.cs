using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Repostiory.IRepostiory;

public interface IUserRepository
{
    bool IsUnique(string username);
    
    Task<LoginRequestDTO> Login(LoginRequestDTO loginRequestDTO);
    
    Task<LocalUser> Register(RegistertionRequestDTO registertionRequestDTO);
    
}