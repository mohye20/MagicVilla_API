using System.Net;
using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repostiory.IRepostiory;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;

[Route("api/villaNumberAPI")]
[ApiController]
public class VillaNumberApiController : ControllerBase
{
    private readonly APIResponse _response;
    private readonly IMapper _mapper;
    private readonly IVillaNumbersRepository _dbVillaNumbers;
    private readonly IVillaRepository _dbVilla;


    public VillaNumberApiController(APIResponse Response, IMapper mapper, IVillaNumbersRepository dbVillaNumbers,
        IVillaRepository dbVilla)
    {
        _response = Response;
        _mapper = mapper;
        _dbVillaNumbers = dbVillaNumbers;
        _dbVilla = dbVilla;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetVillaNumbers()
    {
        try
        {
            IEnumerable<VillaNumber> villaNumbersList = await _dbVillaNumbers.GetAllAsync(includeProperties:"Villa");
            _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumbersList);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = new List<string>() { e.ToString() };
        }

        return _response;
    }


    [HttpGet("{id:int}", Name = "GetVillaNumber")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
    {
        try
        {
            if (id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
            }

            var villa = await _dbVillaNumbers.GetAsync(U => U.VillaNo == id);

            if (villa is null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            _response.Result = _mapper.Map<VillaNumberDTO>(villa);
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = new List<string>() { e.ToString() };
        }

        return _response;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO villaNumberCreateDto)
    {
        try
        {
            if (await _dbVillaNumbers.GetAsync(U => U.VillaNo == villaNumberCreateDto.VillaNo) is not null)
            {
                ModelState.AddModelError(string.Empty, "Villa number already exists");
                return BadRequest(ModelState);
            }

            if (await _dbVilla.GetAsync(U => U.Id == villaNumberCreateDto.VillaId) is null)
            {
                ModelState.AddModelError(string.Empty,"Villa Id is Invalid!");
                return BadRequest(ModelState);
            }

            if (villaNumberCreateDto is null)
            {
                return BadRequest(villaNumberCreateDto);
            }

            var villaNumber = _mapper.Map<VillaNumber>(villaNumberCreateDto);
            await _dbVillaNumbers.CreateAsync(villaNumber);

            _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
            _response.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetVillaNumber", new { id = villaNumber.VillaNo }, _response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = new List<string>() { e.ToString() };
        }

        return _response;
    }

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
    public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var villa = await _dbVillaNumbers.GetAsync(u => u.VillaNo == id);
            if (villa is null)
            {
                return NotFound();
            }

            await _dbVillaNumbers.RemoveAsync(villa);
            _response.StatusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = new List<string>() { e.ToString() };
        }

        return _response;
    }


    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
    public async Task<ActionResult<APIResponse>> UpdatevillaNumber(int id,
        [FromBody] VillaNumberUpdateDTO villaNumberUpdateDto)
    {
        try
        {
            if (villaNumberUpdateDto is null || id != villaNumberUpdateDto.VillaNo)
            {
                return BadRequest();
            }

            if (await _dbVilla.GetAsync(U => U.Id == villaNumberUpdateDto.VillaId) is null)
            {
                ModelState.AddModelError(string.Empty,"Villa Id is Invalid!");
                return BadRequest(ModelState);
            }

            var villaNumber = _mapper.Map<VillaNumber>(villaNumberUpdateDto);

            await _dbVillaNumbers.UpdateAsync(villaNumber);
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessage = new List<string>() { e.ToString() };
        }

        return _response;
    }
}