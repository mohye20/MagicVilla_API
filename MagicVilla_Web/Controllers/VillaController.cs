using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
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

    public async Task<IActionResult> CreateVilla()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
    {
        if (ModelState.IsValid)
        {
            var reponse = await _villaServices.CreateAsync<APIResponse>(model,HttpContext.Session.GetString(SD.SesionToken));
            if (reponse is not null && reponse.IsSuccess)
            {
                TempData["success"] = "Villa Created Successfully";
                return RedirectToAction(nameof(IndexVilla));
            }
        }
        
        TempData["Error"] = "Villa Not Created";

        return View(model);
    }

    [Authorize(Roles = "admin")]

    public async Task<IActionResult> UpdateVilla(int villaId)
    {
        var response = await _villaServices.GetAsync<APIResponse>(villaId,HttpContext.Session.GetString(SD.SesionToken));
        if (response is not null && response.IsSuccess)
        {
            VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
            return View(_mapper.Map<VillaUpdateDTO>(model));
        }

        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]

    public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
    {
        if (ModelState.IsValid)
        {
                TempData["success"] = "Villa Updated Successfully";
            var response = await _villaServices.UpdateAsync<APIResponse>(model,HttpContext.Session.GetString(SD.SesionToken));
            if (response is not null && response.IsSuccess)
            {

                return RedirectToAction(nameof(IndexVilla));
            }
        }
        
        TempData["Error"] = "Villa Not Updated";

        return View(model);
    }

    [Authorize(Roles = "admin")]

    public async Task<IActionResult> DeleteVilla(int VillaId)
    {
        var response = await _villaServices.GetAsync<APIResponse>(VillaId,HttpContext.Session.GetString(SD.SesionToken));
        if (response is not null && response.IsSuccess)
        {
            var model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
            return View(model);
        }

        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteVilla(VillaDTO model)
    {
        var response = await _villaServices.DeleteAsync<APIResponse>(model.Id,HttpContext.Session.GetString(SD.SesionToken));
        if (response is not null && response.IsSuccess)
        {
            TempData["success"] = "Villa Deleted Successfully";
            return RedirectToAction(nameof(IndexVilla));
        }
        TempData["Error"] = "Villa Not Deleted";


        return View(model);
    }

    // GET
    public async Task<IActionResult> IndexVilla()
    {
        List<VillaDTO> list = new();
        var response = await _villaServices.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SesionToken));
        if (response is not null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
        }

        return View(list);
    }
}