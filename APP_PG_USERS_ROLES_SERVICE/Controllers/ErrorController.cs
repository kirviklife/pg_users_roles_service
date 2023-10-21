using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Abstractions;

namespace APP_PG_USERS_ROLES_SERVICE.Controllers
{
	[Route("error")]
	public class ErrorController : Controller
	{

		[Route("404")]
		public IActionResult PageNotFound()
		{
			return View();
		}

        [Route("503")]
        public IActionResult Eror503()
        {
            return View();
        }

		[Route("500")]
		public IActionResult Eror500()
		{
			return View();
		}
	}
}
