using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services;

public class AuthServices : BaseService, IAuthServices
{
    private readonly IHttpClientFactory _clientFactory;
    private string _villaURl;

    public AuthServices(IHttpClientFactory httpClientFactory, IConfiguration Configuration) : base(httpClientFactory)
    {
        _clientFactory = httpClientFactory;
        _villaURl = Configuration.GetValue<string>("ServicesUrls:VillaAPI");
    }

    public Task<T> LoginAsync<T>(LoginRequestDTO obj)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = obj,
            Url = _villaURl + "/api/UserAuth/Login"
        });
    }

    public Task<T> RegisterAsync<T>(RegisterationRequestDTO obj)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = obj,
            Url = _villaURl + "/api/UserAuth/Register"
        });
    }
}