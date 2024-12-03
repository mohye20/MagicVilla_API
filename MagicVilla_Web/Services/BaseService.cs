using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;

namespace MagicVilla_Web.Services;

public class BaseService : IBaseService
{
    public APIResponse responseModel { get; set; }

    public IHttpClientFactory HttpClientFactory { get; set; }

    public BaseService(IHttpClientFactory httpClientFactory)
    {
        HttpClientFactory = httpClientFactory;
        this.responseModel = new();
    }

    public async Task<T> SendAsync<T>(APIRequest request)
    {
        try
        {
            var client = HttpClientFactory.CreateClient("MagicAPI");
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(request.Url);
            if (request.Data is not null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(request.Data),
                    Encoding.UTF8, "application/json");
            }

            switch (request.ApiType)
            {
                case SD.ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case SD.ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case SD.ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }

            HttpResponseMessage apiResponse = null;
            apiResponse = await client.SendAsync(message);
            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            try
            {
                APIResponse ApiResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                if (apiResponse.StatusCode == HttpStatusCode.BadRequest ||
                    apiResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    ApiResponse.StatusCode = HttpStatusCode.BadRequest;
                    ApiResponse.IsSuccess = false;
                    var res = JsonConvert.SerializeObject(ApiResponse);
                    return JsonConvert.DeserializeObject<T>(res);
                }
            }
            catch (Exception e)
            {
                var ApiResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return ApiResponse;
            }

            var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
            return APIResponse;
        }
        catch (Exception e)
        {
            var dto = new APIResponse()
            {
                ErrorMessage = new List<string> { Convert.ToString(e.Message) },
                IsSuccess = false,
            };
            var res = JsonConvert.SerializeObject(dto);
            var APIResponse = JsonConvert.DeserializeObject<T>(res);
            return APIResponse;
        }
    }
}