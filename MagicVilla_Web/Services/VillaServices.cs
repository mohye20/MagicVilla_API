using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services;

public class VillaServices : BaseService, IVillaServices
{
    private readonly IHttpClientFactory _httpClientFactory;
    private string VillaUrl;

    public VillaServices(IHttpClientFactory httpClientFactory, IConfiguration Configuration) : base(httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        VillaUrl = Configuration.GetValue<string>("ServicesUrls:VillaAPI");
    }

    public Task<T> GetAllAsync<T>()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = VillaUrl + "/api/VillaAPI",
        });
    }

    public Task<T> GetAsync<T>(int Id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = VillaUrl + "/api/VillaAPI/" + Id,
        });
    }

    public Task<T> CreateAsync<T>(VillaCreateDTO entity)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = entity,
            Url = VillaUrl + "/api/VillaAPI"
        });
    }

    public Task<T> UpdateAsync<T>(VillaUpdateDTO entity)
    {
        return SendAsync<T>(
            new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Url = VillaUrl + "/api/VillaAPI/" + entity.Id,
                Data = entity,
            });
    }

    public Task<T> DeleteAsync<T>(int Id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.DELETE,
            Url = VillaUrl + "/api/VillaAPI/" + Id
        });
    }
}