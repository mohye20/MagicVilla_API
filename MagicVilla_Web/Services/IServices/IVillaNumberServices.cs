using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices;

public interface IVillaNumberServices
{
    Task<T> GetAllAsync<T>();
    Task<T> GetAsync<T>(int Id);
    Task<T> CreateAsync<T>(VillaNumberCreateDTO Entity);
    Task<T> UpdateAsync<T>(VillaNumberUpdateDTO Entity);
    Task<T> DeleteAsync<T>(int Id);
}