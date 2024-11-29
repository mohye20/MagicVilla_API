using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
	public static class VillaStore
	{

		public static List<VillaDTO> VillaList = new List<VillaDTO>
			{
				new VillaDTO { Id = 1, Name = "Pool View" , Occupancy = 4 , Details="",Amenity="",ImageURL="",Rate = 0,Sqft=0},
			};
	}
}
