using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices;

public interface IVillaServices
{
    Task<T> GetAllAsync<T>();
    Task<T> GetAsync<T>(int Id);
    Task<T> CreateAsync<T>(VillaCreateDTO entity);
    Task<T> UpdateAsync<T>(VillaUpdateDTO entity);
    Task<T> DeleteAsync<T>(int Id);
}