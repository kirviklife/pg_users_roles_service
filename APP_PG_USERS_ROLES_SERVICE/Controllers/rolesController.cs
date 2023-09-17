using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APP_PG_USERS_ROLES_SERVICE.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

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
						var q2 = _context.Database.ExecuteSqlRaw($"select * from update_list_roles();");
					}
                }
				return Ok();
			}
			return PartialView(roles);
		}

		public IActionResult CreateUser()
		{
			return new PartialViewResult
			{
				ViewName = "CreateUser",
				ViewData = new ViewDataDictionary<roles>(ViewData, new roles { })
			};
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CreateUser([Bind("id_role,role_name,role_pass,email,phone,fam,im,otch,is_new_password,is_login,is_superuser,is_createdb,is_createrole,is_inherit,is_replication,valid_until")] roles roles)
		{
            if (roles.fam == null)
            {
                ModelState.AddModelError("fam", "Необходимо указать фамилию");
            }
            else if (roles.im == null)
            {
                ModelState.AddModelError("im", "Необходимо указать имя");
            }
            else if (roles.email == null)
            {
                ModelState.AddModelError("email", "Необходимо указать отчество");
            }
            else if (roles.phone == null)
            {
                ModelState.AddModelError("phone", "Необходимо указать номер моб. телефона");
            }
            else if (roles.role_pass != null)
            {
                if (ModelState.IsValid)
                {
                    roles.id_role = Guid.NewGuid();
                    _context.Add(roles);
                    _context.SaveChanges();
                }
            }
            else
            {
                ModelState.AddModelError("role_pass", "Необходимо указать пароль");
            }
			
			return new PartialViewResult
			{
				ViewName = "CreateUser",
				ViewData = new ViewDataDictionary<roles>(ViewData, new roles { })
			};
		}

		// GET: roles/Edit/5
		public async Task<IActionResult> Edit(Guid? id)
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
            return View(roles);
        }

        // POST: roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id_role,role_name,role_pass,email,phone,fam,im,otch,is_new_password,is_login,is_superuser,is_createdb,is_createrole,is_inherit,is_replication,valid_until")] roles roles)
        {
            if (id != roles.id_role)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roles);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            return View(roles);
        }

        // GET: roles/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
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

        // POST: roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.roles == null)
            {
                return Problem("Entity set 'DataContext.roles'  is null.");
            }
            var roles = await _context.roles.FindAsync(id);
            if (roles != null)
            {
                _context.roles.Remove(roles);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool rolesExists(Guid id)
        {
          return (_context.roles?.Any(e => e.id_role == id)).GetValueOrDefault();
        }
    }
}
