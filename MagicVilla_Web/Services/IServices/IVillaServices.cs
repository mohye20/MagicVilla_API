using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices;

public interface IVillaServices
{
    Task<T> GetAllAsync<T>(string token);
    Task<T> GetAsync<T>(int Id, string token);
    Task<T> CreateAsync<T>(VillaCreateDTO entity, string token);
    Task<T> UpdateAsync<T>(VillaUpdateDTO entity, string token);
    Task<T> DeleteAsync<T>(int Id, string token);
}