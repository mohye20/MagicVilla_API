using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaNumberController : Controller
{
    private readonly IMapper _mapper;
    private readonly IVillaNumberServices _villaNumberServices;
    private readonly IVillaServices _villaServices;

    public VillaNumberController(IMapper mapper, IVillaNumberServices villaNumberServices, IVillaServices villaServices)
    {
        _mapper = mapper;
        _villaNumberServices = villaNumberServices;
        _villaServices = villaServices;
    }

    // GET
    public async Task<IActionResult> IndexVillaNumber()
    {
        List<VillaNumberDTO> list = new List<VillaNumberDTO>();
        var response = await _villaNumberServices.GetAllAsync<APIResponse>();
        if (response is not null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
        }

        return View(list);
    }

    public async Task<IActionResult> CreateVillaNumber()
    {
        VillaNumberCreateVM villaNumberVM = new();
        var response = await _villaServices.GetAllAsync<APIResponse>();
        if (response is not null && response.IsSuccess)
        {
            villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                .Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
        }

        return View(villaNumberVM);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNumberServices.CreateAsync<APIResponse>(model.VillaNumber);
            if (response is not null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }

            else
            {
                if (response.ErrorMessage.Count() > 0)
                {
                    ModelState.AddModelError("ErrorMessages", response.ErrorMessage.FirstOrDefault());
                }
            }
        }

        var respo = await _villaServices.GetAllAsync<APIResponse>();
        if (respo is not null && respo.IsSuccess)
        {
            model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                (Convert.ToString(respo.Result)).Select(I => new SelectListItem
            {
                Text = I.Name,
                Value = I.Id.ToString()
            });
        }


        return View(model);
    }
}