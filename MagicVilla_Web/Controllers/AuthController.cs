using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class AuthController : Controller
{
    private readonly IAuthServices _authServices;

    public AuthController(IAuthServices authServices)
    {
        _authServices = authServices;
    }

    [HttpGet]
    public IActionResult Login()
    {
        LoginRequestDTO obj = new();
        return View(obj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequestDTO obj)
    {
        APIResponse response = await _authServices.LoginAsync<APIResponse>(obj);
        if (response is not null && response.IsSuccess)
        {
            var model = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));
            HttpContext.Session.SetString(SD.SesionToken, model.Token);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError("ErrorMessage", response.ErrorMessage.FirstOrDefault());
            return View(obj);
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterationRequestDTO obj)
    {
        var Result = await _authServices.RegisterAsync<APIResponse>(obj);
        if (Result is null && Result.IsSuccess)
        {
            return RedirectToAction("Login");
        }

        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        HttpContext.Session.SetString(SD.SesionToken, "");
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}