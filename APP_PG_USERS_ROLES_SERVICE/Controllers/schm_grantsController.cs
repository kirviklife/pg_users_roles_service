using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APP_PG_USERS_ROLES_SERVICE.Models;
using Microsoft.AspNetCore.Authorization;

namespace APP_PG_USERS_ROLES_SERVICE.Controllers
{
	[Authorize]
	public class schm_grantsController : Controller
    {
        private readonly DataContext _context;

        public schm_grantsController(DataContext context)
        {
            _context = context;
        }

        // GET: schm_grants/Create
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
            ViewData["schm_grant_privs_id"] = new SelectList(_context.schm_grant_privs, "id_schm_grant_privs", "schm_grant_priv_name");
			ViewBag.DBID = db;
			schm_grants schm_grants = new schm_grants();
			return PartialView("Create", schm_grants);
		}

        // POST: schm_grants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(schm_grants schm_grants)
        {
            ModelState.Remove("roles");
            ModelState.Remove("schemas");
            ModelState.Remove("schm_grant_privs");
            if (ModelState.IsValid)
            {
                int schm_grants_count = _context.schm_grants.Where(s => s.schm_id == schm_grants.schm_id && s.role_id == schm_grants.role_id && s.schm_grant_privs_id == schm_grants.schm_grant_privs_id).Count();
                if (schm_grants_count == 0)
                {
                    schm_grants.id_schm_grants = Guid.NewGuid();
                    _context.Add(schm_grants);
                    await _context.SaveChangesAsync();
                    return Ok("Права добавлены");
                }
                else
                {
                    var schm_grants2 = _context.schm_grants.Where(s => s.schm_id == schm_grants.schm_id && s.role_id == schm_grants.role_id && s.schm_grant_privs_id == schm_grants.schm_grant_privs_id).FirstOrDefault();
                    schm_grants2.is_success = false;
                    schm_grants2.date_time_exec = null;
                    _context.Update(schm_grants2);
                    _context.SaveChanges();
                    return Ok("Будет выполнено повторное назначение прав");
                }
                
            }
            ViewData["role_id"] = new SelectList(_context.roles, "id_role", "id_role", schm_grants.role_id);
            ViewData["schm_id"] = new SelectList(_context.schemas, "id_schm", "id_schm", schm_grants.schm_id);
            ViewData["schm_grant_privs_id"] = new SelectList(_context.schm_grant_privs, "id_schm_grant_privs", "id_schm_grant_privs", schm_grants.schm_grant_privs_id);
			return BadRequest("Произошла ошибка при обработке вашего запроса");
		}

        // GET: schm_grants/Delete/5
        public async Task<IActionResult> RevokeGrant(Guid? id)
        {
            if (id == null || _context.schm_grants == null)
            {
                return NotFound();
            }

            var schm_grants = await _context.schm_grants
                .Include(s => s.roles)
                .Include(s => s.schemas)
                .Include(s => s.schm_grant_privs)
                .FirstOrDefaultAsync(m => m.id_schm_grants == id);
			return PartialView("RevokeGrant", schm_grants);
		}

        // POST: schm_grants/Delete/5
        [HttpPost, ActionName("RevokeGrant")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.schm_grants == null)
            {
				return BadRequest("Произошла ошибка при обработке вашего запроса");
			}
            var schm_grants = await _context.schm_grants.Include(s => s.roles)
                .Include(s => s.schemas)
                .Include(s => s.schm_grant_privs)
                .Include(s => s.schemas.databases)
                .FirstOrDefaultAsync(m => m.id_schm_grants == id);
            if (schm_grants != null)
            {
				var revoke = _context.Database.ExecuteSqlRaw($"Select update_revoke_schm_typical_grants('{schm_grants.id_schm_grants}')");
				var listgrants = _context.Database.ExecuteSqlRaw($"select update_list_schemas_typical_grants()");
            }
            
			return Ok($"Права на {schm_grants.schm_grant_privs.schm_grant_priv_name} в БД {schm_grants.schemas.databases.db_name} для схемы {schm_grants.schemas.schm_name} отняты у роли {schm_grants.roles.role_name} ");
		}

        private bool schm_grantsExists(Guid id)
        {
          return (_context.schm_grants?.Any(e => e.id_schm_grants == id)).GetValueOrDefault();
        }
    }
}
