using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APP_PG_USERS_ROLES_SERVICE.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Authorization;

namespace APP_PG_USERS_ROLES_SERVICE.Controllers
{
    public class rolesController : Controller
    {
        private readonly DataContext _context;

        public rolesController(DataContext context)
        {
            _context = context;
        }

        // GET: roles
        public async Task<IActionResult> Index()
        {
              return _context.roles != null ? 
                          View(await _context.roles.ToListAsync()) :
                          Problem("Entity set 'DataContext.roles'  is null.");
        }

        // GET: roles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.roles == null)
            {
                return NotFound();
            }

            var roles = await _context.roles
                .FirstOrDefaultAsync(m => m.id_role == id);
            if (roles == null)
            {
                return NotFound();
            }

            return View(roles);
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
             if (roles.email == null)
            {
                return BadRequest("Необходимо указать эл. адрес почты");
            }
             if (roles.phone == null)
            {
                return BadRequest("Необходимо указать номер моб. телефона");
            }
            if (roles.role_pass == null)
            { 
                return BadRequest("Необходимо указать пароль");
            }
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
            if (roles.email == null)
            {
                return BadRequest("Необходимо указать эл. адрес почты");
            }
            if (roles.phone == null)
            {
                return BadRequest("Необходимо указать номер моб. телефона");
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
        public ActionResult Delete(Guid? id,Guid srvid)
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

        // POST: roles/Delete/5
        [HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(Guid id, string srvi)
        {
            if (_context.roles == null)
            {
				return BadRequest("Произошла ошибка");
			}
            var upd = _context.Database.ExecuteSqlRaw($"select fnc_del_role('{srvi}','{id}','one');");
            return Ok();
		}

        private bool rolesExists(Guid id)
        {
          return (_context.roles?.Any(e => e.id_role == id)).GetValueOrDefault();
        }
    }
}
