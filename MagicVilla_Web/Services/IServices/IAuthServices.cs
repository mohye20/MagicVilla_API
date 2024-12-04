using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices;

public interface IAuthServices
{
    Task<T> LoginAsync<T>(LoginRequestDTO objToCreate);
    Task<T> RegisterAsync<T>(RegisterationRequestDTO objToCreate);
}