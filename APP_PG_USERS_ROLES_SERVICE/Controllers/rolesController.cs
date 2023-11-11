using APP_PG_USERS_ROLES_SERVICE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APP_PG_USERS_ROLES_SERVICE.Controllers
{
    [Authorize]
    public class rolesController : Controller
    {
        private readonly DataContext _context;

        public rolesController(DataContext context)
        {
            _context = context;
        }

        public IActionResult CreateRole(Guid srvid)
        {
            ViewBag.srvid = srvid;
            roles roles = new roles();
            return PartialView("CreateRole", roles);
            //return new PartialViewResult
            //{
            //	ViewName = "CreateRole",
            //	ViewData = new ViewDataDictionary<roles>(ViewData, new roles { })
            //};
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRole(roles roles, string srvid)
        {
            if (roles.role_name == null)
            {
                return BadRequest("Необходимо указать имя роли");
            }
            if (ModelState.IsValid)
            {
                var rl1 = _context.roles.Where(r => r.role_name == roles.role_name).Count();
                if (rl1 == 0)
                {
                    roles.id_role = Guid.NewGuid();
                    _context.Add(roles);
                    _context.SaveChanges();
                    var q1 = _context.Database.ExecuteSqlRaw($"select * from fnc_add_role('{srvid}','{roles.id_role}')");
                    var q2 = _context.Database.ExecuteSqlRaw($"select * from update_list_roles();");
                }
                else
                {
                    var es = _context.srv_roles_relations.Where(s => s.srv_id == Guid.Parse(srvid) && s.roles.role_name == roles.role_name).Count();
                    if (es == 0)
                    {
                        var rl2 = _context.roles.Where(r => r.role_name == roles.role_name).FirstOrDefault();
                        srv_roles_relations srv_Roles_Relations = new srv_roles_relations();
                        srv_Roles_Relations.id_srv_role = Guid.NewGuid();
                        srv_Roles_Relations.srv_id = Guid.Parse(srvid);
                        srv_Roles_Relations.role_id = rl2.id_role;
                        srv_Roles_Relations.oid_roles = null;
                        _context.Add(srv_Roles_Relations);
                        _context.SaveChanges();
                        var q1 = _context.Database.ExecuteSqlRaw($"select * from fnc_add_role('{srvid}','{rl2.id_role}')");
                        var q2 = _context.Database.ExecuteSqlRaw($"select * from fnc_upd_roles();");
                        var q3 = _context.Database.ExecuteSqlRaw($"select * from update_list_roles();");


                    }
                }
                return Ok();
            }
            return BadRequest("Произошла ошибка при обработке вашего запроса");
        }

        public IActionResult CreateUser(Guid srvid)
        {
            ViewBag.srvid = srvid;
            roles roles = new roles();
            return PartialView("CreateUser", roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(roles roles, string srvid, bool isrole)
        {
            if (roles.role_name == null)
            {
                return BadRequest("Необходимо придумать логин");
            }
            var est = _context.roles.Where(r => r.role_name == roles.role_name).Count();
            if (!isrole)
            {
                if (est > 0)
                {
                    return BadRequest("Пользователь с таким логином существует");
                }
                if (roles.fam == null)
                {
                    return BadRequest("Необходимо указать фамилию");
                }
                if (roles.im == null)
                {
                    return BadRequest("Необходимо указать имя");
                }
                if (roles.role_pass == null)
                {
                    return BadRequest("Необходимо указать пароль");
                }
            }
            if (ModelState.IsValid)
            {
                var rl1 = _context.roles.Where(r => r.role_name == roles.role_name).Count();
                if (rl1 == 0 && isrole)
                {
					return BadRequest("Такой роли не существует");
				}
                if (rl1 == 0)
                {
                    roles.id_role = Guid.NewGuid();
                    _context.Add(roles);
                    _context.SaveChanges();
                    var q1 = _context.Database.ExecuteSqlRaw($"select * from fnc_add_role('{srvid}','{roles.id_role}')");
                    var q2 = _context.Database.ExecuteSqlRaw($"select * from update_list_roles();");
                }
                else
                {
                    var es = _context.srv_roles_relations.Where(s => s.srv_id == Guid.Parse(srvid) && s.roles.role_name == roles.role_name).Count();
                    if (es == 0)
                    {
                        var rl2 = _context.roles.Where(r => r.role_name == roles.role_name).FirstOrDefault();
                        srv_roles_relations srv_Roles_Relations = new srv_roles_relations();
                        srv_Roles_Relations.id_srv_role = Guid.NewGuid();
                        srv_Roles_Relations.srv_id = Guid.Parse(srvid);
                        srv_Roles_Relations.role_id = rl2.id_role;
                        srv_Roles_Relations.oid_roles = null;
                        _context.Add(srv_Roles_Relations);
                        _context.SaveChanges();
                        var q1 = _context.Database.ExecuteSqlRaw($"select * from fnc_add_role('{srvid}','{rl2.id_role}')");
                        var q2 = _context.Database.ExecuteSqlRaw($"select * from fnc_upd_roles();");
                        var q3 = _context.Database.ExecuteSqlRaw($"select * from update_list_roles();");


                    }
                }
                return Ok();
            }
            return BadRequest("Произошла ошибка при обработке вашего запроса");

        }

        public async Task<IActionResult> EditUser(Guid id, Guid srvid)
        {
            ViewBag.sr = srvid;
            if (id == null || _context.roles == null)
            {
                return NotFound();
            }

            var roles = await _context.roles.FindAsync(id);
            if (roles == null)
            {
                return NotFound();
            }
            return View(roles);
        }

        [HttpPost("/Roles/EditUser/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(Guid id, roles roles, string srvi)
        {
            if (id != roles.id_role)
            {
                return NotFound();
            }
            if (roles.role_name == null)
            {
                return BadRequest("Необходимо указать имя роли");
            }
            if (roles.fam == null)
            {
                return BadRequest("Необходимо указать фамилию");
            }
            if (roles.im == null)
            {
                return BadRequest("Необходимо указать имя");
            }
            if (roles.role_pass == null)
            {
                return BadRequest("Необходимо указать пароль");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roles);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!rolesExists(roles.id_role))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var upd = _context.Database.ExecuteSqlRaw($"select fnc_upd_role('{srvi}','{id}');");
                return Ok(new { result = "Redirect", url = Url.Action("index", "databases", new { id = srvi }) });
            }
            return BadRequest("Произошла ошибка");
        }

        public async Task<IActionResult> EditRole(Guid id, Guid srvid)
        {
            ViewBag.sr = srvid;
            if (id == null || _context.roles == null)
            {
                return NotFound();
            }

            var roles = await _context.roles.FindAsync(id);
            if (roles == null)
            {
                return NotFound();
            }
            return View(roles);
        }


        [HttpPost("/Roles/EditRole/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole(Guid id, roles roles, string srvi)
        {
            if (id != roles.id_role)
            {
                return NotFound();
            }
            if (roles.role_name == null)
            {
                return BadRequest("Необходимо указать имя роли");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roles);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!rolesExists(roles.id_role))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var upd = _context.Database.ExecuteSqlRaw($"select fnc_upd_role('{srvi}','{id}');");
                return Ok(new { result = "Redirect", url = Url.Action("index", "databases", new { id = srvi }) });
            }
            return BadRequest("Произошла ошибка");
        }

        // GET: roles/Delete/5
        public ActionResult Delete(Guid? id, Guid srvid)
        {
            if (id == null || _context.roles == null)
            {
                return NotFound();
            }
            ViewBag.srvid = srvid;
            var roles = _context.roles
                .FirstOrDefault(m => m.id_role == id);
            if (roles == null)
            {
                return NotFound();
            }

            return PartialView("Delete", roles);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id, string srvi)
        {
            if (_context.roles == null)
            {
                return BadRequest("Произошла ошибка");
            }
            var upd = _context.Database.ExecuteSqlRaw($"select fnc_del_role('{srvi}','{id}','one');");
			return Ok("Пользователь/Роль удален(а)");
		}

        private bool rolesExists(Guid id)
        {
            return (_context.roles?.Any(e => e.id_role == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Current = "roles";
            rolesdata rolesdata = new rolesdata
            {
                roles_users_servers = await _context.roles_users_servers.ToListAsync(),
                roles = await _context.roles.ToListAsync()
			};
                              
            return View(rolesdata);
        }

        public async Task<IActionResult> IndexLog()
        {
            ViewBag.Current = "log_not";
            var dataContext = _context.view_log_not_typical_grants;
            return View(await dataContext.ToListAsync());
        }

        public async Task<ActionResult> GetSearchLog_not(string SearchValue, int SearchValue2)
        {
            List<view_log_not_typical_grants> view_log_not_typical_grants = new List<view_log_not_typical_grants>();
            try
            {
                if (string.IsNullOrEmpty(SearchValue))
                {
                    switch (SearchValue2)
                    {
                        case 0:
                            view_log_not_typical_grants = await _context.view_log_not_typical_grants.ToListAsync();
                            return PartialView("GetSearchLog_not", view_log_not_typical_grants);
                        case 1:
                            view_log_not_typical_grants = await _context.view_log_not_typical_grants.Where(s => s.is_success).ToListAsync();
                            return PartialView("GetSearchLog_not", view_log_not_typical_grants);
                        case 2:
                            view_log_not_typical_grants = await _context.view_log_not_typical_grants.Where(s => s.is_success == false).ToListAsync();
                            return PartialView("GetSearchLog_not", view_log_not_typical_grants);
                        default:
                            view_log_not_typical_grants = await _context.view_log_not_typical_grants.ToListAsync();
                            return PartialView("GetSearchLog_not", view_log_not_typical_grants);
                    }

                }
                else
                {
                    switch (SearchValue2)
                    {
                        case 0:
                            view_log_not_typical_grants = await _context.view_log_not_typical_grants.Where(d => EF.Functions.Like(d.srv_name, "%" + SearchValue + "%") || EF.Functions.Like(d.ipadd, "%" + SearchValue + "%") || EF.Functions.Like(d.db_name, "%" + SearchValue + "%") || EF.Functions.Like(d.schm_name, "%" + SearchValue + "%") || EF.Functions.Like(d.date_time_exec.ToString(), "%" + SearchValue + "%") || EF.Functions.Like(d.zadanie, "%" + SearchValue + "%") || EF.Functions.Like(d.task_name, "%" + SearchValue + "%")).ToListAsync();
                            return PartialView("GetSearchLog_not", view_log_not_typical_grants);
                        case 1:
                            view_log_not_typical_grants = await _context.view_log_not_typical_grants.Where(d => d.is_success && (EF.Functions.Like(d.srv_name, "%" + SearchValue + "%") || EF.Functions.Like(d.ipadd, "%" + SearchValue + "%") || EF.Functions.Like(d.db_name, "%" + SearchValue + "%") || EF.Functions.Like(d.schm_name, "%" + SearchValue + "%") || EF.Functions.Like(d.date_time_exec.ToString(), "%" + SearchValue + "%") || EF.Functions.Like(d.zadanie, "%" + SearchValue + "%") || EF.Functions.Like(d.task_name, "%" + SearchValue + "%"))).ToListAsync();
                            return PartialView("GetSearchLog_not", view_log_not_typical_grants);
                        case 2:
                            view_log_not_typical_grants = await _context.view_log_not_typical_grants.Where(d => d.is_success == false && (EF.Functions.Like(d.srv_name, "%" + SearchValue + "%") || EF.Functions.Like(d.ipadd, "%" + SearchValue + "%") || EF.Functions.Like(d.db_name, "%" + SearchValue + "%") || EF.Functions.Like(d.schm_name, "%" + SearchValue + "%") || EF.Functions.Like(d.date_time_exec.ToString(), "%" + SearchValue + "%") || EF.Functions.Like(d.zadanie, "%" + SearchValue + "%") || EF.Functions.Like(d.task_name, "%" + SearchValue + "%"))).ToListAsync();
                            return PartialView("GetSearchLog_not", view_log_not_typical_grants);
                        default:
                            view_log_not_typical_grants = await _context.view_log_not_typical_grants.ToListAsync();
                            return PartialView("GetSearchLog_not", view_log_not_typical_grants);
                    }

                }
            }
            catch
            {
                view_log_not_typical_grants = await _context.view_log_not_typical_grants.ToListAsync();
                return PartialView("GetSearchLog_not", view_log_not_typical_grants);
            }
        }

        public ActionResult IndexScheduler()
        {
            return View();
        }
        public JsonResult GetEvents()
        {

            var events = _context.roles.ToList();
            return new JsonResult(events);

        }

        [HttpGet]
        public async Task<ActionResult> GetRoles(string Search)
        {
            if (Search == null || Search == "")
            {
                var rol = await _context.roles.Where(r => r.is_login == false && r.fam == null).ToListAsync();
                return PartialView("GetRoles", rol);
            }
            else
            {

                var rol = await _context.roles.Where(r => r.is_login == false && r.fam == null && EF.Functions.Like(r.role_name, "%" + Search + "%")).ToListAsync();
                return PartialView("GetRoles", rol);
            }

        }

        [HttpGet]
        public async Task<ActionResult> GetUsers(string Search)
        {
            if (Search == null || Search == "")
            {
                var rol = await _context.roles.Where(r => r.is_login || r.fam != null).ToListAsync();
                return PartialView("GetUsers", rol);
            }
            else
            {
                var rol = await _context.roles.Where(r => (r.is_login || r.fam != null) && (EF.Functions.Like(r.role_name, "%" + Search + "%") || EF.Functions.Like(r.fam, "%" + Search + "%") || EF.Functions.Like(r.im, "%" + Search + "%") || EF.Functions.Like(r.otch, "%" + Search + "%") || EF.Functions.Like(r.phone, "%" + Search + "%") || EF.Functions.Like(r.email, "%" + Search + "%"))).ToListAsync();
                return PartialView("GetUsers", rol);
            }

        }

		[HttpGet]
		public async Task<ActionResult> GetRolesUsersServers(string Search)
		{
			if (Search == null || Search == "")
			{
				var rol = await _context.roles_users_servers.ToListAsync();
				return PartialView("GetRolesUsersServers", rol);
			}
			else
			{

				var rol = await _context.roles_users_servers.Where(r => EF.Functions.Like(r.role_name, "%" + Search + "%") || EF.Functions.Like(r.fio, "%" + Search + "%") || EF.Functions.Like(r.phone, "%" + Search + "%") || EF.Functions.Like(r.email, "%" + Search + "%") || EF.Functions.Like(r.servers_list, "%" + Search + "%")).ToListAsync();
				return PartialView("GetRolesUsersServers", rol);
			}

		}

		public IActionResult CreateNewRole()
        {
            roles roles = new roles();
            return PartialView("CreateNewRole", roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewRole(roles roles)
        {
            if (roles.role_name == null)
            {
                return BadRequest("Необходимо указать имя роли");
            }
            if (ModelState.IsValid)
            {
                var rl1 = _context.roles.Where(r => r.role_name == roles.role_name).Count();
                if (rl1 == 0)
                {
                    roles.id_role = Guid.NewGuid();
                    _context.Add(roles);
                    _context.SaveChanges();
                }
                else
                {
                    return BadRequest("Такая роль уже существует");
                }
                return Ok("Роль добавлена");
            }
            return BadRequest("Произошла ошибка при обработке вашего запроса");
        }

        public IActionResult CreateNewUser()
        {
            roles roles = new roles();
            return PartialView("CreateNewUser", roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewUser(roles roles)
        {
            if (roles.role_name == null)
            {
                return BadRequest("Необходимо придумать логин");
            }
            var est = _context.roles.Where(r => r.role_name == roles.role_name).Count();

            if (est > 0)
            {
                return BadRequest("Пользователь с таким логином существует");
            }
            if (roles.fam == null)
            {
                return BadRequest("Необходимо указать фамилию");
            }
            if (roles.im == null)
            {
                return BadRequest("Необходимо указать имя");
            }
            if (roles.role_pass == null)
            {
                return BadRequest("Необходимо указать пароль");
            }
            if (ModelState.IsValid)
            {
                roles.id_role = Guid.NewGuid();
                _context.Add(roles);
                _context.SaveChanges();
                var q2 = _context.Database.ExecuteSqlRaw($"select * from update_list_roles();");
                return Ok("Пользователь добавлен");
            }
            return BadRequest("Произошла ошибка при обработке вашего запроса");
        }
		public async Task<IActionResult> EditModalUser(Guid id)
		{
			if (id == null || _context.roles == null)
			{
				return NotFound();
			}

			var roles = await _context.roles.FindAsync(id);
			if (roles == null)
			{
				return NotFound();
			}
			return PartialView("EditModalUser", roles);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditModalUser(Guid id, roles roles)
		{
			if (id != roles.id_role)
			{
				return NotFound();
			}
			if (roles.role_name == null)
			{
				return BadRequest("Необходимо указать имя роли");
			}
			if (roles.fam == null)
			{
				return BadRequest("Необходимо указать фамилию");
			}
			if (roles.im == null)
			{
				return BadRequest("Необходимо указать имя");
			}
			if (roles.role_pass == null)
			{
				return BadRequest("Необходимо указать пароль");
			}
			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(roles);
					_context.SaveChanges();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!rolesExists(roles.id_role))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
                var srv_Roles_ = _context.srv_roles_relations.Where(s=>s.role_id == id).ToList();
                foreach (srv_roles_relations s in srv_Roles_)
                {
					_context.Database.ExecuteSqlRaw($"select fnc_upd_role('{s.srv_id}','{id}');");
				}    
				return Ok("Данные пользователя обновлены");
			}
			return BadRequest("Произошла ошибка");
		}

		public async Task<IActionResult> EditModalRole(Guid id)
		{
			if (id == null || _context.roles == null)
			{
				return NotFound();
			}

			var roles = await _context.roles.FindAsync(id);
			if (roles == null)
			{
				return NotFound();
			}
			return PartialView("EditModalRole", roles);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditModalRole(Guid id, roles roles)
		{
			if (id != roles.id_role)
			{
				return NotFound();
			}
			if (roles.role_name == null)
			{
				return BadRequest("Необходимо указать имя роли");
			}
			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(roles);
					_context.SaveChanges();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!rolesExists(roles.id_role))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				var srv_Roles_ = _context.srv_roles_relations.Where(s => s.role_id == id).ToList();
				foreach (srv_roles_relations s in srv_Roles_)
				{
					_context.Database.ExecuteSqlRaw($"select fnc_upd_role('{s.srv_id}','{id}');");
				}
				return Ok("Данные роли обновлены");
			}
			return BadRequest("Произошла ошибка");
		}
		public ActionResult DeleteAll(Guid? id)
		{
			if (id == null || _context.roles == null)
			{
				return NotFound();
			}
			var roles = _context.roles
				.FirstOrDefault(m => m.id_role == id);
			if (roles == null)
			{
				return NotFound();
			}

			return PartialView("DeleteAll", roles);
		}


		[HttpPost, ActionName("DeleteAll")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmedAll(Guid id)
		{
			if (_context.roles == null)
			{
				return BadRequest("Произошла ошибка");
			}
			var upd = _context.Database.ExecuteSqlRaw($"select fnc_del_role('00000000-0000-0000-0000-000000000000','{id}','all');");
            var roles = _context.roles.Find(id);
            if (roles != null)
            {
                _context.roles.Remove(roles);
            }

            _context.SaveChanges();
            return Ok("Пользователь/Роль удален(а)");
		}
	}
}