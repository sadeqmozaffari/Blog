using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
	[Route("api/Category")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		[HttpGet]
		public string Get()
		{
			return "Hello Sadeq";
		}
	}
}
