using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repostiory.IRepostiory;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
	[Route("api/VillaAPI")]
	[ApiController]
	public class VillaAPIController : ControllerBase
	{
		private readonly IVillaRepository _dbVilla;
		private readonly IMapper _mapper;

		public VillaAPIController( IVillaRepository dbVilla , IMapper mapper)
		{
			_dbVilla = dbVilla;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
		{

			IEnumerable<Villa> VillaList = await _dbVilla.GetAllAsync();
			return Ok(_mapper.Map<List<VillaDTO>>(VillaList));
		}

		[HttpGet("{Id}", Name = "GetVilla")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<VillaDTO>> GetVilla(int Id)
		{
			if (Id == 0)
			{
				return BadRequest();
			}

			var Villa = await _dbVilla.GetAsync(V => V.Id == Id);

			if (Villa is null)
			{
				return NotFound();
			}
			return Ok(_mapper.Map<VillaDTO>(Villa));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villaDTO)
		{
			if (await _dbVilla.GetAsync(V => V.Name.ToLower() == villaDTO.Name.ToLower()) is not null)
			{
				ModelState.AddModelError(string.Empty, "Villa Already Exist");
				return BadRequest(ModelState);
			}

			if (villaDTO is null)
			{
				return StatusCode(StatusCodes.Status400BadRequest);
			}

			//if (villaDTO.Id > 0)
			//{
			//	return StatusCode(StatusCodes.Status500InternalServerError);
			//}

			//Villa Model = new()
			//{
			//	Name = villaDTO.Name,
			//	Amenity = villaDTO.Amenity,
			//	Details = villaDTO.Details,
			//	ImageURL = villaDTO.ImageURL,
			//	Rate = villaDTO.Rate,
			//	Sqft = villaDTO.Sqft,
			//	Occupancy = villaDTO.Occupancy,
			//	CreatedDate = DateTime.Now
			//};

			Villa Model = _mapper.Map<Villa>(villaDTO);

			await _dbVilla.CreateAsync(Model);
			return CreatedAtRoute("GetVilla", new { id = Model.Id }, Model);
		}

		[HttpDelete("{Id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> DeleteVilla(int Id)
		{
			if (Id <= 0)
			{
				return BadRequest();
			}

			var Villa = await _dbVilla.GetAsync(V => V.Id == Id);
			if (Villa is null)
			{
				ModelState.AddModelError(string.Empty, $"No Villa With this Id {Id}");
				return BadRequest(ModelState);
			}

			await _dbVilla.RemoveAsync(Villa);


			return NoContent();
		}

		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[HttpPut("{Id}", Name = "UpdateVilla")]
		public async Task<IActionResult> UpdateVilla(int Id, [FromBody] VillaUpdateDTO villaDTO)
		{
			if (Id != villaDTO.Id || villaDTO is null)
			{
				return BadRequest();
			}

			Villa Model = _mapper.Map<Villa>(villaDTO);
			await _dbVilla.UpdateAsync(Model);
			return NoContent();
		}

		[HttpPatch("{Id}")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> UpdatePartialVilla(int Id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
		{
			if (patchDTO is null || Id <= 0)
			{
				return BadRequest();
			}

			var Villa = await _dbVilla.GetAsync(V => V.Id == Id,Tracked:false);

			if (Villa is null)
			{
				return BadRequest();
			}

			VillaUpdateDTO villaDTO =   _mapper.Map<VillaUpdateDTO>(Villa);

			patchDTO.ApplyTo(villaDTO, ModelState);
			Villa Model =  _mapper.Map<Villa>(villaDTO);

			await _dbVilla.UpdateAsync(Model);


			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			return StatusCode(StatusCodes.Status204NoContent);
		}
	}
}