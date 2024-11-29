using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
	[Route("api/VillaAPI")]
	[ApiController]
	public class VillaAPIController : ControllerBase
	{
		private readonly ILogging _logger;

		public VillaAPIController(ILogging logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public IEnumerable<VillaDTO> GetVillas()
		{
			_logger.Log("Getting All Villas", string.Empty);
			return VillaStore.VillaList;
		}

		[HttpGet("{Id}", Name = "GetVilla")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<VillaDTO> GetVilla(int Id)
		{
			if (Id == 0)
			{
				_logger.Log($"Getting Villa Error With Id {Id}", "error");
				return BadRequest();
			}

			var Villa = VillaStore.VillaList.FirstOrDefault(V => V.Id == Id);

			if (Villa is null)
			{
				return NotFound();
			}
			return Ok(Villa);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
		{
			if (VillaStore.VillaList.FirstOrDefault(V => V.Name.ToLower() == villaDTO.Name.ToLower()) is not null)
			{
				ModelState.AddModelError(string.Empty, "Villa Already Exist");
				return BadRequest(ModelState);
			}

			if (villaDTO is null)
			{
				return StatusCode(StatusCodes.Status400BadRequest);
			}

			if (villaDTO.Id > 0)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}

			villaDTO.Id = VillaStore.VillaList.OrderByDescending(V => V.Id).FirstOrDefault().Id + 1;
			VillaStore.VillaList.Add(villaDTO);

			return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
		}

		[HttpDelete("{Id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult DeleteVilla(int Id)
		{
			if (Id <= 0)
			{
				return BadRequest();
			}

			var Villa = VillaStore.VillaList.FirstOrDefault(V => V.Id == Id);
			if (Villa is null)
			{
				return BadRequest();
			}

			VillaStore.VillaList.Remove(Villa);

			return NoContent();
		}

		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[HttpPut("{Id}", Name = "UpdateVilla")]
		public IActionResult UpdateVilla(int Id, [FromBody] VillaDTO villaDTO)
		{
			if (Id != villaDTO.Id || villaDTO is null)
			{
				return BadRequest();
			}

			var Villa = VillaStore.VillaList.FirstOrDefault(V => V.Id == villaDTO.Id);
			Villa.Name = villaDTO.Name;
			Villa.Sqft = villaDTO.Sqft;
			Villa.Occupancy = villaDTO.Occupancy;

			return NoContent();
		}

		[HttpPatch("{Id}")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public ActionResult UpdatePartialVilla(int Id, JsonPatchDocument<VillaDTO> patchDTO)
		{
			if (patchDTO is null || Id <= 0)
			{
				return BadRequest();
			}

			var Villa = VillaStore.VillaList.FirstOrDefault(V => V.Id == Id);

			if (Villa is null)
			{
				return BadRequest();


			}

			patchDTO.ApplyTo(Villa, ModelState);

			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			return StatusCode(StatusCodes.Status204NoContent);
		}
	}
}