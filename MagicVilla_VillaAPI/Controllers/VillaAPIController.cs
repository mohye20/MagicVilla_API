using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repostiory.IRepostiory;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;
        protected APIResponse APiResponse;

        public VillaAPIController(IVillaRepository dbVilla, IMapper mapper)
        {
            _dbVilla = dbVilla;
            _mapper = mapper;
            this.APiResponse = new();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                IEnumerable<Villa> VillaList = await _dbVilla.GetAllAsync();
                APiResponse.Result = _mapper.Map<List<VillaDTO>>(VillaList);
                APiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(APiResponse);
            }
            catch (Exception ex)
            {
                APiResponse.IsSuccess = false;
                APiResponse.ErrorMessage = new List<string>() { ex.ToString() };
            }

            return APiResponse;
        }

        [HttpGet("{Id}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int Id)
        {
            try
            {
                if (Id == 0)
                {
                    APiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(APiResponse);
                }

                var Villa = await _dbVilla.GetAsync(V => V.Id == Id);

                if (Villa is null)
                {
                    APiResponse.StatusCode = HttpStatusCode.BadRequest;
                    APiResponse.ErrorMessage = new List<string>() { $"No Villa With this Id {Id}" };
                    return NotFound(APiResponse);
                }

                APiResponse.Result = _mapper.Map<VillaDTO>(Villa);
                APiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(APiResponse);
            }
            catch (Exception ex)
            {
                APiResponse.IsSuccess = false;
                APiResponse.ErrorMessage = new List<string>() { ex.ToString() };
            }

            return APiResponse;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO villaDTO)
        {
            try
            {
                if (await _dbVilla.GetAsync(V => V.Name.ToLower() == villaDTO.Name.ToLower()) is not null)
                {
                    ModelState.AddModelError(string.Empty, "Villa Already Exist");
                    return BadRequest(ModelState);
                }

                if (villaDTO is null)
                {
                    APiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                Villa Model = _mapper.Map<Villa>(villaDTO);

                await _dbVilla.CreateAsync(Model);
                APiResponse.StatusCode = HttpStatusCode.OK;
                APiResponse.Result = _mapper.Map<VillaDTO>(Model);
                return CreatedAtRoute("GetVilla", new { id = Model.Id }, Model);
            }
            catch (Exception ex)
            {
                APiResponse.IsSuccess = false;
                APiResponse.ErrorMessage = new List<string>() { ex.ToString() };
            }

            return APiResponse;
        }

        [HttpDelete("{Id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int Id)
        {
            try
            {
                if (Id <= 0)
                {
                    APiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(APiResponse);
                }

                var Villa = await _dbVilla.GetAsync(V => V.Id == Id);
                if (Villa is null)
                {
                    ModelState.AddModelError(string.Empty, $"No Villa With this Id {Id}");
                    APiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(ModelState);
                }

                await _dbVilla.RemoveAsync(Villa);

                APiResponse.StatusCode = HttpStatusCode.NoContent;
                APiResponse.IsSuccess = true;
                return Ok(APiResponse);
            }

            catch (Exception ex)
            {
                APiResponse.IsSuccess = false;
                APiResponse.ErrorMessage = new List<string>() { ex.ToString() };
            }


            return APiResponse;
        }

        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("{Id}", Name = "UpdateVilla")]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int Id, [FromBody] VillaUpdateDTO villaDTO)
        {
            try
            {
                if (Id != villaDTO.Id || villaDTO is null)
                {
                    return BadRequest();
                }

                Villa Model = _mapper.Map<Villa>(villaDTO);
                await _dbVilla.UpdateAsync(Model);
                APiResponse.StatusCode = HttpStatusCode.NoContent;
                APiResponse.IsSuccess = true;
                return Ok(APiResponse);
            }

            catch (Exception ex)
            {
                APiResponse.IsSuccess = false;
                APiResponse.ErrorMessage = new List<string>() { ex.ToString() };
            }

            return APiResponse;
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{Id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePartialVilla(int Id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO is null || Id <= 0)
            {
                return BadRequest();
            }

            var Villa = await _dbVilla.GetAsync(V => V.Id == Id, Tracked: false);

            if (Villa is null)
            {
                return BadRequest();
            }

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(Villa);

            patchDTO.ApplyTo(villaDTO, ModelState);
            Villa Model = _mapper.Map<Villa>(villaDTO);

            await _dbVilla.UpdateAsync(Model);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}