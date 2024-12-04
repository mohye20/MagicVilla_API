using System.Net;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repostiory.IRepostiory;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;

[Route("api/v{version:apiVersion}/UserAuth")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserRepository _userRepository;
    private APIResponse _response;

    public UserController(IUserRepository UserRepository)
    {
        _userRepository = UserRepository;
        this._response = new APIResponse();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
    {
        var LoginResponse = await _userRepository.Login(model);
        if (LoginResponse.User is null || string.IsNullOrEmpty(LoginResponse.Token))
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessage.Add("Username Or Password is incorrect");
            return BadRequest(_response);
        }

        _response.IsSuccess = true;
        _response.StatusCode = HttpStatusCode.OK;
        _response.Result = LoginResponse;
        return Ok(_response);
    }


    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegistertionRequestDTO model)
    {
        bool ifUserNameUnique = _userRepository.IsUnique(model.UserName);

        if (ifUserNameUnique == false)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessage.Add("Username is already taken");
            return BadRequest(_response);
        }

        var user = await _userRepository.Register(model);

        if (user is null)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessage.Add("Error Wthile Registering");
            return BadRequest(_response);
        }

        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        return Ok(_response);
    }
}