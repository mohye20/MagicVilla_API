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
		private readonly ApplicationDbContext _db;

		public VillaAPIController(ILogging logger, ApplicationDbContext db)
		{
			_logger = logger;
			_db = db;
		}

		[HttpGet]
		public ActionResult<IEnumerable<VillaDTO>> GetVillas()
		{
			_logger.Log("Getting All Villas", string.Empty);
			return Ok(_db.Villas.ToList());
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

			var Villa = _db.Villas.FirstOrDefault(V => V.Id == Id);

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
			if (_db.Villas.FirstOrDefault(V => V.Name.ToLower() == villaDTO.Name.ToLower()) is not null)
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

			_db.Villas.Add(Model);
			_db.SaveChanges();
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

			var Villa = _db.Villas.FirstOrDefault(V => V.Id == Id);
			if (Villa is null)
			{
				return BadRequest();
			}

			_db.Villas.Remove(Villa);

			_db.SaveChanges();

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
			_db.SaveChanges();
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

			var Villa = _db.Villas.FirstOrDefault(V => V.Id == Id);

			if (Villa is null)
			{
				return BadRequest();
			}

			VillaDTO villaDTO = new()
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

			_db.SaveChanges();

			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			return StatusCode(StatusCodes.Status204NoContent);
		}
	}
}