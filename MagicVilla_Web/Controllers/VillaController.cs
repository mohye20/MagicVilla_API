using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaController : Controller
{
    private readonly IVillaServices _villaServices;
    private readonly IMapper _mapper;

    public VillaController(IVillaServices villaServices, IMapper Mapper)
    {
        _villaServices = villaServices;
        _mapper = Mapper;
    }

    // GET
    public async Task<IActionResult> IndexVilla()
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