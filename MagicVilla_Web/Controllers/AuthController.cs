using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;

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
        return View();
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
        return View();
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}