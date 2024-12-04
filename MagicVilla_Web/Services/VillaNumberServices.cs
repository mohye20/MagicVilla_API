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

    public Task<T> GetAllAsync<T>(string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = VillaUrl + "/api/villaNumberAPI",
            Token = token
        });
    }

    public Task<T> GetAsync<T>(int Id, string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = VillaUrl + "/api/villaNumberAPI/" + Id,
            Token = token
        });
    }

    public Task<T> CreateAsync<T>(VillaNumberCreateDTO Entity, string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Url = VillaUrl + "/api/villaNumberAPI",
            Data = Entity,
            Token = token
        });
    }

    public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO Entity, string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.PUT,
            Data = Entity,
            Url = VillaUrl + "/api/villaNumberAPI/" + Entity.VillaNo,
            Token = token
        });
    }

    public Task<T> DeleteAsync<T>(int Id, string token)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.DELETE,
            Url = VillaUrl + "/api/villaNumberAPI/" + Id,
            Token = token
        });
    }
}