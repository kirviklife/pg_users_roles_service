using APP_PG_USERS_ROLES_SERVICE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace APP_PG_USERS_ROLES_SERVICE.Controllers
{
	public class databasesController : Controller
	{
		private readonly DataContext _context;

		public databasesController(DataContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index(Guid id)
		{
			ViewBag.Current = "Servers";
			var dataContext = new SrvData
			{
				databases = await _context.databases.Where(d => d.srv_id == id).ToListAsync(),
				srv_roles_relations = await _context.srv_roles_relations.Include(d => d.roles).Include(d => d.servers).Where(r => r.srv_id == id).ToListAsync(),
				users_roles_relation = await _context.users_roles_relation.ToListAsync(),
				v_users_roles_grants = await _context.v_users_roles_grants.FromSql($"SELECT * FROM fnc_users_roles_grants ({id})").ToListAsync()
			};
			ViewBag.ID_Srv = id;
			ViewBag.Srv_Name = _context.servers.Where(s => s.id_srv == id).Select(s => s.srv_name).FirstOrDefault();
			return View(dataContext);
		}

		public JsonResult GetSearchDB(string SearchValue, Guid iddb)
		{
			List<databases> databases = new List<databases>();
			try
			{
				if (string.IsNullOrEmpty(SearchValue))
				{
					databases = _context.databases.Where(d => d.srv_id == iddb).ToList();
					return Json(databases);
				}
				else
				{
					databases = _context.databases.Where(d => EF.Functions.Like(d.db_name, "%" + SearchValue + "%") && d.srv_id == iddb).ToList();
					return Json(databases);
				}
			}
			catch
			{
				databases = _context.databases.Where(d => d.srv_id == iddb).ToList();
				return Json(databases);
			}
		}

		public JsonResult GetSearchRole(string SearchValue, Guid iddb)
		{

			try
			{
				if (string.IsNullOrEmpty(SearchValue))
				{
					var role_srch = (from rl_sr in _context.srv_roles_relations
									 join rl in _context.roles
									 on rl_sr.role_id equals rl.id_role
									 where rl_sr.srv_id == iddb && rl.is_login == false
									 select new { rl.role_name, rl.id_role, rl_sr.oid_roles, rl_sr.srv_id }).ToList();
					return Json(role_srch);
				}
				else
				{
					var role_srch = (from rl_sr in _context.srv_roles_relations
									 join rl in _context.roles
									 on rl_sr.role_id equals rl.id_role
									 where EF.Functions.Like(rl.role_name, "%" + SearchValue + "%")
									 && rl_sr.srv_id == iddb && rl.is_login == false
									 select new { rl.role_name, rl.id_role, rl_sr.oid_roles, rl_sr.srv_id }).ToList();
					return Json(role_srch);
				}
			}
			catch
			{
				var role_srch = (from rl_sr in _context.srv_roles_relations
								 join rl in _context.roles
								 on rl_sr.role_id equals rl.id_role
								 where rl_sr.srv_id == iddb && rl.is_login == false
								 select new { rl.role_name, rl.id_role, rl_sr.oid_roles, rl_sr.srv_id }).ToList();
				return Json(role_srch);
			}
		}

		public JsonResult GetUpdateRoles(Guid iddb)
		{
			try
			{
				var q3 = _context.Database.ExecuteSqlRaw($"select * from update_list_roles();");
				var role_srch = (from rl_sr in _context.srv_roles_relations
								 join rl in _context.roles
								 on rl_sr.role_id equals rl.id_role
								 where rl_sr.srv_id == iddb && rl.is_login == false
								 select new { rl.role_name, rl.id_role, rl_sr.oid_roles, rl_sr.srv_id }).ToList();
				return Json(role_srch);
			}
			catch
			{
				var q3 = _context.Database.ExecuteSqlRaw($"select * from update_list_roles();");
				var role_srch = (from rl_sr in _context.srv_roles_relations
								 join rl in _context.roles
								 on rl_sr.role_id equals rl.id_role
								 where rl_sr.srv_id == iddb && rl.is_login == false
								 select new { rl.role_name, rl.id_role, rl_sr.oid_roles, rl_sr.srv_id }).ToList();
				return Json(role_srch);
			}
		}


		public JsonResult GetSearchUser(string SearchValue, Guid iddb)
		{
			try
			{
				if (string.IsNullOrEmpty(SearchValue))
				{
					var role_srch = (from rl_sr in _context.srv_roles_relations
									 join rl in _context.roles
									 on rl_sr.role_id equals rl.id_role
									 where rl_sr.srv_id == iddb && rl.is_login == true
									 select new { rl.role_name, rl.id_role, rl_sr.oid_roles, rl.fam, rl.im, rl.otch, rl_sr.srv_id }).ToList();
					return Json(role_srch);
				}
				else
				{
					var role_srch = (from rl_sr in _context.srv_roles_relations
									 join rl in _context.roles
									 on rl_sr.role_id equals rl.id_role
									 where rl_sr.srv_id == iddb && rl.is_login == true && (EF.Functions.Like(rl.role_name, "%" + SearchValue + "%") || EF.Functions.Like(rl.fam, "%" + SearchValue + "%") || EF.Functions.Like(rl.im, "%" + SearchValue + "%") || EF.Functions.Like(rl.otch, "%" + SearchValue + "%"))
									 select new { rl.role_name, rl.id_role, rl_sr.oid_roles, rl.fam, rl.im, rl.otch, rl_sr.srv_id }).ToList();
					return Json(role_srch);
				}
			}
			catch
			{
				var role_srch = (from rl_sr in _context.srv_roles_relations
								 join rl in _context.roles
								 on rl_sr.role_id equals rl.id_role
								 where rl_sr.srv_id == iddb && rl.is_login == true
								 select new { rl.role_name, rl.id_role, rl_sr.oid_roles, rl.fam, rl.im, rl.otch, rl_sr.srv_id }).ToList();
				return Json(role_srch);
			}
		}


		public JsonResult GetUpdateUsers(Guid iddb)
		{
			try
			{
				var q3 = _context.Database.ExecuteSqlRaw($"select * from update_list_roles();");
				var role_srch = (from rl_sr in _context.srv_roles_relations
								 join rl in _context.roles
								 on rl_sr.role_id equals rl.id_role
								 where rl_sr.srv_id == iddb && rl.is_login == true
								 select new { rl.role_name, rl.id_role, rl_sr.oid_roles, rl.fam, rl.im, rl.otch, rl_sr.srv_id }).ToList();
				return Json(role_srch);

			}
			catch
			{
				var q3 = _context.Database.ExecuteSqlRaw($"select * from update_list_roles();");
				var role_srch = (from rl_sr in _context.srv_roles_relations
								 join rl in _context.roles
								 on rl_sr.role_id equals rl.id_role
								 where rl_sr.srv_id == iddb && rl.is_login == true
								 select new { rl.role_name, rl.id_role, rl_sr.oid_roles, rl.fam, rl.im, rl.otch, rl_sr.srv_id }).ToList();
				return Json(role_srch);
			}
		}

		public JsonResult GetUpdateDB(Guid iddb)
		{
			List<databases> databases = new List<databases>();
			try
			{

				var styleCodeID = _context.Database.ExecuteSqlRaw("Select update_list_databases()");
				databases = _context.databases.Where(d => d.srv_id == iddb).ToList();
				return Json(databases);
			}
			catch
			{
				databases = _context.databases.Where(d => d.srv_id == iddb).ToList();
				return Json(databases);
			}
		}


		public async Task<IActionResult> Details(Guid? id)
		{
			ViewBag.Current = "Servers";
			var dataContext = new DBData
			{
				databases = await _context.databases.Where(d => d.id_db == id).ToListAsync(),
				schemas = await _context.schemas.Where(s => s.db_id == id).ToListAsync(),
				db_grants = await _context.db_grants.Where(g => g.db_id == id).Include(d => d.databases).Include(d => d.db_grant_privs).Include(d => d.roles).ToListAsync(),
				db_grant_privs = await _context.db_grant_privs.ToListAsync(),
				tasks_not_typical_grants = await _context.tasks_not_typical_grants.ToListAsync(),
				not_typical_grants = await _context.not_typical_grants.Where(n => n.schemas.db_id == id).ToListAsync(),
				schm_grants = await _context.schm_grants.Where(s => s.schemas.db_id == id).Include(d => d.schemas).Include(d => d.schm_grant_privs).Include(d => d.roles).ToListAsync(),
				schm_grant_privs = await _context.schm_grant_privs.ToListAsync()
			};
			ViewBag.ID_DB = id;
			ViewBag.DB_Name = _context.databases.Where(s => s.id_db == id).Select(s => s.db_name).FirstOrDefault();
			ViewBag.ID_Srv = _context.databases.Where(s => s.id_db == id).Select(s => s.servers.id_srv).FirstOrDefault();
			ViewBag.Srv_Name = _context.databases.Where(s => s.id_db == id).Select(s => s.servers.srv_name).FirstOrDefault();

			return View(dataContext);
		}


		[HttpGet]
		public async Task<ActionResult> GetUsersRoles(Guid id_s, string SearchRelRole, int selectRel)
		{
			IQueryable<v_users_roles_grants> order = _context.v_users_roles_grants.FromSql($"SELECT * FROM fnc_users_roles_grants ({id_s})");
			switch (selectRel)
			{
				case 0:
					order = order.Where(o => EF.Functions.Like(o.role, "%" + SearchRelRole + "%") || EF.Functions.Like(o.member_name, "%" + SearchRelRole + "%") || EF.Functions.Like(o.path, "%" + SearchRelRole + "%"));
					break;
				case 1:
					order = order.Where(o => EF.Functions.Like(o.member_name, "%" + SearchRelRole + "%"));
					break;
				case 2:
					order = order.Where(o => EF.Functions.Like(o.role, "%" + SearchRelRole + "%"));
					break;
				case 3:
					order = order.Where(o => EF.Functions.Like(o.path, "%" + SearchRelRole + "%"));
					break;
				default:
					order = order.Where(o => EF.Functions.Like(o.role, "%" + SearchRelRole + "%") || EF.Functions.Like(o.member_name, "%" + SearchRelRole + "%") || EF.Functions.Like(o.path, "%" + SearchRelRole + "%"));
					break;
			}

			return PartialView("GetUsersRoles", order);
		}


		[HttpGet]
		public async Task<ActionResult> GetGrantsDb(Guid iddb, string SearchDB)
		{
			if (SearchDB == null || SearchDB == "")
			{
				var db_grants = await _context.db_grants.Where(g => g.db_id == iddb).Include(d => d.databases).Include(d => d.db_grant_privs).Include(d => d.roles).ToListAsync();
				return PartialView("GetGrantsDb", db_grants);
			}
			else
			{
				var db_grants = await _context.db_grants.Where(g => g.db_id == iddb && (EF.Functions.Like(g.databases.db_name, "%" + SearchDB + "%") || EF.Functions.Like(g.roles.role_name, "%" + SearchDB + "%") || EF.Functions.Like(g.db_grant_privs.db_grant_priv_name, "%" + SearchDB + "%"))).Include(d => d.databases).Include(d => d.db_grant_privs).Include(d => d.roles).ToListAsync();
				return PartialView("GetGrantsDb", db_grants);
			}

		}

		[HttpGet]
		public async Task<ActionResult> GetGrantsSchema(Guid iddb, string SearchDB)
		{
			if (SearchDB == null || SearchDB == "")
			{
				var db_grants = await _context.schm_grants.Where(s => s.schemas.db_id == iddb).Include(d => d.schemas).Include(d => d.schm_grant_privs).Include(d => d.roles).ToListAsync();
				return PartialView("GetGrantsSchema", db_grants);
			}
			else
			{
				var db_grants = await _context.schm_grants.Where(s => s.schemas.db_id == iddb && (EF.Functions.Like(s.schemas.schm_name, "%" + SearchDB + "%")|| EF.Functions.Like(s.roles.role_name, "%" + SearchDB + "%") || EF.Functions.Like(s.schm_grant_privs.schm_grant_priv_name, "%" + SearchDB + "%"))).Include(d => d.schemas).Include(d => d.schm_grant_privs).Include(d => d.roles).ToListAsync();
				return PartialView("GetGrantsSchema", db_grants);
			}

		}

		[HttpGet]
		public async Task<ActionResult> GetSchemas(Guid iddb, string SearchDB)
		{
			if (SearchDB == null || SearchDB == "")
			{
				var db_grants = await _context.schemas.Where(s => s.db_id == iddb).ToListAsync();
				return PartialView("GetSchemas", db_grants);
			}
			else
			{
				var db_grants = await _context.schemas.Where(s => s.db_id == iddb && EF.Functions.Like(s.schm_name, "%" + SearchDB + "%")).ToListAsync();
				return PartialView("GetSchemas", db_grants);
			}

		}

		[HttpGet]
		public async Task<ActionResult> GetTasks(string SearchDB)
		{
			if (SearchDB == null || SearchDB == "")
			{
				var db_grants = await _context.tasks_not_typical_grants.ToListAsync();
				return PartialView("GetTasks", db_grants);
			}
			else
			{
				var db_grants = await _context.tasks_not_typical_grants.Where(g=> EF.Functions.Like(g.task_name, "%" + SearchDB + "%") || EF.Functions.Like(g.task_descr, "%" + SearchDB + "%") || EF.Functions.Like(g.task_script, "%" + SearchDB + "%")).ToListAsync();
				return PartialView("GetTasks", db_grants);
			}
				

		}

		[HttpGet]
		public async Task<ActionResult> GetNotTasks(Guid iddb, string SearchDB)
		{
			if (SearchDB == null || SearchDB == "")
			{
				var db_grants = await _context.not_typical_grants.Include(n => n.roles)
				.Include(n => n.schemas)
				.Include(n => n.tasks_not_typical_grants).Where(n=>n.schemas.db_id == iddb).ToListAsync();
				return PartialView("GetNotTasks", db_grants);
			}
			else
			{
				var db_grants = await _context.not_typical_grants.Include(n => n.roles)
				.Include(n => n.schemas)
				.Include(n => n.tasks_not_typical_grants).Where(g => g.schemas.db_id == iddb &&( EF.Functions.Like(g.tasks_not_typical_grants.task_name, "%" + SearchDB + "%") || EF.Functions.Like(g.roles.role_name, "%" + SearchDB + "%") || EF.Functions.Like(g.schemas.schm_name, "%" + SearchDB + "%"))).ToListAsync();
				return PartialView("GetTasks", db_grants);
			}


		}

		private bool databasesExists(Guid id)
		{
			return (_context.databases?.Any(e => e.id_db == id)).GetValueOrDefault();
		}
	}
}
