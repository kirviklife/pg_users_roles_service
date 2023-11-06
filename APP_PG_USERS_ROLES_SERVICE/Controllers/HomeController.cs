using APP_PG_USERS_ROLES_SERVICE.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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

		[Authorize]
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
			int countpolz_ispolz = _context.srv_roles_relations.Include(n => n.roles).Select(s => new
			{
				s.roles.id_role,
				s.roles.is_login
			}
			).Distinct().Where(s => s.is_login).Count();
			int countrl_ispolz = _context.srv_roles_relations.Include(n => n.roles).Select(s => new
			{
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
			var jobs_status = _context.jobs_status.OrderBy(s => s.my_day).ToList();
			var grants_status = _context.grants_status.ToList();

			var StackedViewModel = new StackedViewModel
			{
				StatRoles = StatRo
				,
				StatServers = StatServers,
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
			ClaimsPrincipal claims = HttpContext.User;
			if (claims.Identity.IsAuthenticated)
				return RedirectToAction("Index", "Home");
			auth_model auth_model = new auth_model();
			return View(auth_model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Autorize(auth_model auth_model)
		{
			var us = await _context.roles.Where(r => r.role_name == auth_model.signin_email && r.is_superuser == true).FirstOrDefaultAsync();
			if (us != null)
			{
				try
				{
					//string connectionString = $"Host=192.168.56.102;Integrated Security=false;Username={auth_model.signin_email};Password={auth_model.signin_password};Database=pg_users_roles_service";
					//NpgsqlConnection connection = new NpgsqlConnection(connectionString);
					//await connection.OpenAsync();
					//await connection.CloseAsync();
					_context.Database.SetConnectionString($"Host=192.168.56.102;Integrated Security=false;Username={auth_model.signin_email};Password={auth_model.signin_password};Database=pg_users_roles_service");
					await _context.Database.OpenConnectionAsync();
					await _context.Database.CloseConnectionAsync();
					List<Claim> claims = new List<Claim>()
					{
						new Claim(ClaimTypes.NameIdentifier, auth_model.signin_email),
						new Claim(ClaimsIdentity.DefaultRoleClaimType, "superuser"),
						new Claim("OtherProperties","Exaple Role")
					};
					ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					AuthenticationProperties properties = new AuthenticationProperties()
					{
						AllowRefresh = true,
						IsPersistent = true
					};
					HttpContext.Session.SetString("log", auth_model.signin_email);
					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
					return RedirectToAction("Index", "Home");
				}
				catch 
				{
					ViewBag.osh = "Подключиться к БД не удалось";
				}

			}
			return View(auth_model);
		}

		public async Task<IActionResult> LogOut()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Autorize", "Home");
		}

	}
}