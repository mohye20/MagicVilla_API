using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices;

public interface IVillaNumberServices
{
    Task<T> GetAllAsync<T>(string token);
    Task<T> GetAsync<T>(int Id, string token);
    Task<T> CreateAsync<T>(VillaNumberCreateDTO Entity, string token);
    Task<T> UpdateAsync<T>(VillaNumberUpdateDTO Entity, string token);
    Task<T> DeleteAsync<T>(int Id , string token);
}