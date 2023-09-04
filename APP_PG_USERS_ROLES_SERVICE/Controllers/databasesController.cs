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
									 select new { rl.role_name, rl.id_role, rl_sr.oid_roles }).ToList();
					return Json(role_srch);
				}
				else
				{
					var role_srch = (from rl_sr in _context.srv_roles_relations
									 join rl in _context.roles
									 on rl_sr.role_id equals rl.id_role
									 where EF.Functions.Like(rl.role_name, "%" + SearchValue + "%")
									 && rl_sr.srv_id == iddb && rl.is_login == false
									 select new { rl.role_name, rl.id_role, rl_sr.oid_roles }).ToList();
					return Json(role_srch);
				}
			}
			catch
			{
				var role_srch = (from rl_sr in _context.srv_roles_relations
								 join rl in _context.roles
								 on rl_sr.role_id equals rl.id_role
								 where rl_sr.srv_id == iddb && rl.is_login == false
								 select new { rl.role_name, rl.id_role, rl_sr.oid_roles }).ToList();
				return Json(role_srch);
			}
		}

		public JsonResult GetSearchUser(string SearchValue, Guid iddb)
		{
			List<roles> roles = new List<roles>();
			try
			{
				if (string.IsNullOrEmpty(SearchValue))
				{
					roles = _context.roles.Where(d => d.is_login == true).ToList();
					return Json(roles);
				}
				else
				{
					roles = _context.roles.Where(d => (EF.Functions.Like(d.role_name, "%" + SearchValue + "%") || EF.Functions.Like(d.fam, "%" + SearchValue + "%") || EF.Functions.Like(d.im, "%" + SearchValue + "%") || EF.Functions.Like(d.otch, "%" + SearchValue + "%")) && d.is_login == true).ToList();
					return Json(roles);
				}
			}
			catch
			{
				roles = _context.roles.Where(d => d.is_login == true).ToList();
				return Json(roles);
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
				schm_grants = await _context.schm_grants.Where(s => s.schemas.db_id == id).ToListAsync(),
				schm_grant_privs = await _context.schm_grant_privs.ToListAsync()
			};
			ViewBag.ID_DB = id;
			ViewBag.DB_Name = _context.databases.Where(s => s.id_db == id).Select(s => s.db_name).FirstOrDefault();
			ViewBag.ID_Srv = _context.databases.Where(s => s.id_db == id).Select(s => s.servers.id_srv).FirstOrDefault();
			ViewBag.Srv_Name = _context.databases.Where(s => s.id_db == id).Select(s => s.servers.srv_name).FirstOrDefault();

			return View(dataContext);
		}


		[HttpGet]
		public async Task<ActionResult> GetUsersRoles(Guid id_s)
		{
			IQueryable<v_users_roles_grants> order = _context.v_users_roles_grants.FromSql($"SELECT * FROM fnc_users_roles_grants ({id_s})");
			return PartialView("GetUsersRoles", order);
		}














































		// GET: databases/Create
		public IActionResult Create()
		{
			ViewData["srv_id"] = new SelectList(_context.servers, "id_srv", "id_srv");
			return View();
		}

		// POST: databases/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("id_db,db_name,srv_id,oid_db")] databases databases)
		{
			ModelState.Remove("servers");
			if (ModelState.IsValid)
			{
				databases.id_db = Guid.NewGuid();
				_context.Add(databases);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["srv_id"] = new SelectList(_context.servers, "id_srv", "id_srv", databases.srv_id);
			return View(databases);
		}

		// GET: databases/Edit/5
		public async Task<IActionResult> Edit(Guid? id)
		{
			if (id == null || _context.databases == null)
			{
				return NotFound();
			}

			var databases = await _context.databases.FindAsync(id);
			if (databases == null)
			{
				return NotFound();
			}
			ViewData["srv_id"] = new SelectList(_context.servers, "id_srv", "id_srv", databases.srv_id);
			return View(databases);
		}

		// POST: databases/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, [Bind("id_db,db_name,srv_id,oid_db")] databases databases)
		{
			if (id != databases.id_db)
			{
				return NotFound();
			}
			ModelState.Remove("servers");
			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(databases);
					await _context.SaveChangesAsync();
				}
				catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
				{
					if (!databasesExists(databases.id_db))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction("Index", new { id = databases.srv_id });
			}
			ViewData["srv_id"] = new SelectList(_context.servers, "id_srv", "id_srv", databases.srv_id);
			return View(databases);
		}

		// GET: databases/Delete/5
		public async Task<IActionResult> Delete(Guid? id)
		{
			if (id == null || _context.databases == null)
			{
				return NotFound();
			}

			var databases = await _context.databases
				.Include(d => d.servers)
				.FirstOrDefaultAsync(m => m.id_db == id);
			if (databases == null)
			{
				return NotFound();
			}

			return View(databases);
		}

		// POST: databases/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			if (_context.databases == null)
			{
				return Problem("Entity set 'DataContext.databases'  is null.");
			}
			var databases = await _context.databases.FindAsync(id);
			if (databases != null)
			{
				_context.databases.Remove(databases);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool databasesExists(Guid id)
		{
			return (_context.databases?.Any(e => e.id_db == id)).GetValueOrDefault();
		}
	}
}
