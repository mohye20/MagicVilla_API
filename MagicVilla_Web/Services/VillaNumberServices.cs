using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services;

public class VillaNumberServices : BaseService, IVillaNumberServices
{
    private readonly IHttpClientFactory _httpClientFactory;
    private string VillaUrl;

    public VillaNumberServices(IHttpClientFactory HttpClientFactory, IConfiguration Configuration) : base(
        HttpClientFactory)
    {
        _httpClientFactory = HttpClientFactory;
        VillaUrl = Configuration.GetValue<string>("ServicesUrls:VillaAPI");
    }

    public Task<T> GetAllAsync<T>()
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = VillaUrl + "/api/villaNumberAPI"
        });
    }

    public Task<T> GetAsync<T>(int Id)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = VillaUrl + "/api/villaNumberAPI/" + Id
        });
    }

    public Task<T> CreateAsync<T>(VillaNumberCreateDTO Entity)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Url = VillaUrl + "/api/villaNumberAPI",
            Data = Entity
        });
    }

    public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO Entity)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT,
            Data = Entity,
            Url = VillaUrl + "/api/villaNumberAPI" + Entity.VillaNo
        });
    }

    public Task<T> DeleteAsync<T>(int Id)
    {
       return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.DELETE,
            Url = VillaUrl + "/api/villaNumberAPI" + Id
        });
    }
}