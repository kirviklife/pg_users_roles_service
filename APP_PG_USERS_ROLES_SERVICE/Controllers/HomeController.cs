using APP_PG_USERS_ROLES_SERVICE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace APP_PG_USERS_ROLES_SERVICE.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly DataContext _context;
		public HomeController(ILogger<HomeController> logger, DataContext context)
		{
			_logger = logger;
			_context = context;
		}


		public ActionResult Index()
		{
			ViewBag.Current = "Home";
			//list of department  
			var StatServers = new List<StatServers>();
			StatServers.Add(new StatServers
			{
				Title = "Работает",
				Quantity = _context.view_servers_connect_checks.Where(s => s.check == "OK").Count()
			});
			StatServers.Add(new StatServers
			{
				Title = "Не работает",
				Quantity = _context.view_servers_connect_checks.Where(s => s.check != "OK").Count()
			});
			
			
			
			var StatRo = new List<StatRoles>();
			int countpolz_ispolz = _context.srv_roles_relations.Include(n => n.roles).Select(s=>new { 
				s.roles.id_role, s.roles.is_login
			}
			).Distinct().Where(s => s.is_login).Count();
			int countrl_ispolz = _context.srv_roles_relations.Include(n => n.roles).Select(s => new {
				s.roles.id_role,
				s.roles.is_login
			}
			).Distinct().Where(s => !s.is_login).Count();
			int countpolz = _context.roles.Where(s => s.is_login).Count();
			int countroles = _context.roles.Where(s => !s.is_login).Count();
			StatRo.Add(new StatRoles
			{
				Title = "Всего пользователей",
				Quantity = countpolz
			});
			StatRo.Add(new StatRoles
			{
				Title = "Всего ролей",
				Quantity = countroles
			});
			StatRo.Add(new StatRoles
			{
				Title = "Используется пользователей",
				Quantity = countpolz_ispolz
			});
			StatRo.Add(new StatRoles
			{
				Title = "Используется ролей",
				Quantity = countrl_ispolz
			});
			StatRo.Add(new StatRoles
			{
				Title = "Не используется пользователей",
				Quantity = countpolz - countpolz_ispolz
			});
			StatRo.Add(new StatRoles
			{
				Title = "Не используется ролей",
				Quantity = countroles - countrl_ispolz
			});
			var jobs_status = _context.jobs_status.OrderBy(s=>s.my_day).ToList();
			var grants_status = _context.grants_status.ToList();

			var StackedViewModel = new StackedViewModel { 
				StatRoles = StatRo
				, StatServers = StatServers,
				jobs_status = jobs_status,
				grants_status = grants_status
			};



			return View(StackedViewModel);

		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		public IActionResult Autorize()
		{
			return View();
		}

	}
}