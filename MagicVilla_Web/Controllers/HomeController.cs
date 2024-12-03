using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class HomeController : Controller
{
    private readonly IVillaServices _villaServices;

    public HomeController(IVillaServices villaServices)
    {
        _villaServices = villaServices;
    }


    public async Task<IActionResult> Index()
    {
        List<VillaDTO> list = new();
        var response = await _villaServices.GetAllAsync<APIResponse>();

        if (response is not null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
        }

        return View(list);
    }
}