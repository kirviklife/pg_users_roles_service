using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APP_PG_USERS_ROLES_SERVICE.Models;
using Azure.Core.GeoJson;

namespace APP_PG_USERS_ROLES_SERVICE.Controllers
{
    public class not_typical_grantsController : Controller
    {
        private readonly DataContext _context;

        public not_typical_grantsController(DataContext context)
        {
            _context = context;
        }

        // GET: not_typical_grants
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.not_typical_grants.Include(n => n.roles).Include(n => n.schemas).Include(n => n.tasks_not_typical_grants);
            return View(await dataContext.ToListAsync());
        }

        // GET: not_typical_grants/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.not_typical_grants == null)
            {
                return NotFound();
            }

            var not_typical_grants = await _context.not_typical_grants
                .Include(n => n.roles)
                .Include(n => n.schemas)
                .Include(n => n.tasks_not_typical_grants)
                .FirstOrDefaultAsync(m => m.id_not_typical_grant == id);
            if (not_typical_grants == null)
            {
                return NotFound();
            }

            return View(not_typical_grants);
        }

        // GET: not_typical_grants/Create
        public IActionResult Create(Guid id, Guid db)
        {
			var rl = from roles in _context.roles
					 join srv_roles_relations in _context.srv_roles_relations on roles.id_role equals srv_roles_relations.role_id
					 where srv_roles_relations.srv_id == id
					 select new
					 {
						 roles.role_name,
						 roles.id_role
					 };
			ViewData["role_id"] = new SelectList(rl, "id_role", "role_name");
            ViewData["schm_id"] = new SelectList(_context.schemas.Where(s=>s.db_id == db), "id_schm", "schm_name");
            ViewData["task_id"] = new SelectList(_context.tasks_not_typical_grants, "id_task", "task_name");
			ViewBag.iddb = db;
			not_typical_grants not_Typical_Grants = new not_typical_grants();
			return PartialView("Create", not_Typical_Grants);
		}

        // POST: not_typical_grants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(not_typical_grants not_typical_grants)
        {
            ModelState.Remove("roles");
            ModelState.Remove("schemas");
            ModelState.Remove("tasks_not_typical_grants");
            if (ModelState.IsValid)
            {
                string strOne = not_typical_grants.parameters[0];
                string[] strArrayOne = new string[] { "" };
                not_typical_grants.parameters = strOne.Split(',');
                not_typical_grants.id_not_typical_grant = Guid.NewGuid();
                _context.Add(not_typical_grants);
                await _context.SaveChangesAsync();
                return Ok();
            }
			return BadRequest("Произошла ошибка при обработке вашего запроса");
		}

        // GET: not_typical_grants/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.not_typical_grants == null)
            {
                return NotFound();
            }

            var not_typical_grants = await _context.not_typical_grants.FindAsync(id);
            if (not_typical_grants == null)
            {
                return NotFound();
            } 
            ViewData["role_id"] = new SelectList(_context.roles, "id_role", "id_role", not_typical_grants.role_id);
            ViewData["schm_id"] = new SelectList(_context.schemas, "id_schm", "id_schm", not_typical_grants.schm_id);
            ViewData["task_id"] = new SelectList(_context.tasks_not_typical_grants, "id_task", "id_task", not_typical_grants.task_id);
            return View(not_typical_grants);
        }

        // POST: not_typical_grants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id_not_typical_grant,task_id,last_date_time_exec,date_time_create,schm_id,is_repeat,parameters,role_id")] not_typical_grants not_typical_grants)
        {
            if (id != not_typical_grants.id_not_typical_grant)
            {
                return NotFound();
            }
            ModelState.Remove("roles");
            ModelState.Remove("schemas");
            ModelState.Remove("tasks_not_typical_grants");
            if (ModelState.IsValid)
            {
                try
                {
                    string strOne = not_typical_grants.parameters[0];
                    string[] strArrayOne = new string[] { "" };
                    not_typical_grants.parameters = strOne.Split(',');
                    _context.Update(not_typical_grants);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!not_typical_grantsExists(not_typical_grants.id_not_typical_grant))
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
            ViewData["role_id"] = new SelectList(_context.roles, "id_role", "id_role", not_typical_grants.role_id);
            ViewData["schm_id"] = new SelectList(_context.schemas, "id_schm", "id_schm", not_typical_grants.schm_id);
            ViewData["task_id"] = new SelectList(_context.tasks_not_typical_grants, "id_task", "id_task", not_typical_grants.task_id);
            return View(not_typical_grants);
        }

        // GET: not_typical_grants/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.not_typical_grants == null)
            {
                return NotFound();
            }

            var not_typical_grants = await _context.not_typical_grants
                .Include(n => n.roles)
                .Include(n => n.schemas)
                .Include(n => n.tasks_not_typical_grants)
                .FirstOrDefaultAsync(m => m.id_not_typical_grant == id);
            if (not_typical_grants == null)
            {
                return NotFound();
            }

            return View(not_typical_grants);
        }

        // POST: not_typical_grants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.not_typical_grants == null)
            {
                return Problem("Entity set 'DataContext.not_typical_grants'  is null.");
            }
            var not_typical_grants = await _context.not_typical_grants.FindAsync(id);
            if (not_typical_grants != null)
            {
                _context.not_typical_grants.Remove(not_typical_grants);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool not_typical_grantsExists(Guid id)
        {
          return (_context.not_typical_grants?.Any(e => e.id_not_typical_grant == id)).GetValueOrDefault();
        }
    }
}
