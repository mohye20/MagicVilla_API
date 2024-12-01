using System.Net;

namespace MagicVilla_VillaAPI.Models
{
	public class APIRespoinse
	{

		public HttpStatusCode StatusCode { get; set; }

		public bool IsSuccess { get; set; } = true;git 

		public List<string> ErrorMessage { get; set; }

		public object Result { get; set; }
	}
}
