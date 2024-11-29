using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
	[Route("api/VillaAPI")]
	[ApiController]
	public class VillaAPIController : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public VillaAPIController( ApplicationDbContext db)
		{
			_db = db;
		}

		[HttpGet]
		public ActionResult<IEnumerable<VillaDTO>> GetVillas()
		{
			return Ok(_db.Villas.ToList());
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

			var Villa = await _db.Villas.FirstOrDefaultAsync(V => V.Id == Id);

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
		public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villaDTO)
		{
			if (await _db.Villas.FirstOrDefaultAsync(V => V.Name.ToLower() == villaDTO.Name.ToLower()) is not null)
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

			Villa Model = new()
			{
				Name = villaDTO.Name,
				Amenity = villaDTO.Amenity,
				Details = villaDTO.Details,
				ImageURL = villaDTO.ImageURL,
				Rate = villaDTO.Rate,
				Sqft = villaDTO.Sqft,
				Occupancy = villaDTO.Occupancy,
				CreatedDate = DateTime.Now
			};

			await _db.Villas.AddAsync(Model);
			await _db.SaveChangesAsync();
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

			var Villa = await _db.Villas.FirstOrDefaultAsync(V => V.Id == Id);
			if (Villa is null)
			{
				ModelState.AddModelError(string.Empty, $"No Villa With this Id {Id}");
				return BadRequest(ModelState);
			}

			_db.Villas.Remove(Villa);

			await _db.SaveChangesAsync();

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

			Villa Model = new()
			{
				Id = villaDTO.Id,
				Name = villaDTO.Name,
				Amenity = villaDTO.Amenity,
				Details = villaDTO.Details,
				ImageURL = villaDTO.ImageURL,
				Rate = villaDTO.Rate,
				Sqft = villaDTO.Sqft,
				Occupancy = villaDTO.Occupancy,
			};

			 _db.Villas.Update(Model);
			await _db.SaveChangesAsync();
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

			var Villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(V => V.Id == Id);

			if (Villa is null)
			{
				return BadRequest();
			}

			VillaUpdateDTO villaDTO = new()
			{
				Id = Villa.Id,
				Name = Villa.Name,
				Amenity = Villa.Amenity,
				Details = Villa.Details,
				ImageURL = Villa.ImageURL,
				Rate = Villa.Rate,
				Sqft = Villa.Sqft,
				Occupancy = Villa.Occupancy,
			};

			patchDTO.ApplyTo(villaDTO, ModelState);

			Villa Model = new()
			{
				Id = villaDTO.Id,
				Name = villaDTO.Name,
				Amenity = villaDTO.Amenity,
				Details = villaDTO.Details,
				ImageURL = villaDTO.ImageURL,
				Rate = villaDTO.Rate,
				Sqft = villaDTO.Sqft,
				Occupancy = villaDTO.Occupancy,
			};

			_db.Villas.Update(Model);

			await _db.SaveChangesAsync();

			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			return StatusCode(StatusCodes.Status204NoContent);
		}
	}
}