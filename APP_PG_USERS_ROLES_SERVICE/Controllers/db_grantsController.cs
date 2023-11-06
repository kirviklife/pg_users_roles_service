using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APP_PG_USERS_ROLES_SERVICE.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

namespace APP_PG_USERS_ROLES_SERVICE.Controllers
{
	[Authorize]
	public class db_grantsController : Controller
    {
        private readonly DataContext _context;

        public db_grantsController(DataContext context)
        {
            _context = context;
        }

        // GET: db_grants/Create
        public ActionResult Create(Guid id, Guid db)
        {
            var rl = from roles in _context.roles
                     join srv_roles_relations in _context.srv_roles_relations on roles.id_role equals srv_roles_relations.role_id
                     where srv_roles_relations.srv_id == id 
                     select new
                     {
                         roles.role_name,
                         roles.id_role
                     };
            ViewData["db_id"] = new SelectList(_context.databases.Where(d=>d.id_db == db), "id_db", "db_name");
            ViewData["db_grant_privs_id"] = new SelectList(_context.db_grant_privs, "id_db_grant_privs", "db_grant_priv_name");
            ViewData["role_id"] = new SelectList(rl, "id_role", "role_name");
            ViewBag.DBID = db;
			db_grants db_grants = new db_grants();
            return PartialView("Create", db_grants);
        }

        // POST: db_grants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Guid id, string dbid, db_grants db_grants)
        {
			ModelState.Remove("roles");
			ModelState.Remove("databases");
			ModelState.Remove("db_grant_privs");
			if (ModelState.IsValid)
            {
                int dg = _context.db_grants.Where(g => g.db_id == db_grants.db_id && g.role_id == db_grants.role_id && g.db_grant_privs_id == db_grants.db_grant_privs_id).Count();
                if (dg > 0)
                {
                    var db_grants2 = _context.db_grants.Where(g => g.db_id == db_grants.db_id && g.role_id == db_grants.role_id && g.db_grant_privs_id == db_grants.db_grant_privs_id).FirstOrDefault();
                    db_grants2.is_success = false;
                    db_grants2.date_time_exec = null;
                    _context.Update(db_grants2);
                    _context.SaveChanges();
                    return Ok("Будет выполнено повторное назначение прав");
                }
                else
                {
                    db_grants.id_db_grants = Guid.NewGuid();
                    _context.Add(db_grants);
                    _context.SaveChanges();
                    return Ok("Права добавлены");
                }
            }
            ViewData["db_id"] = new SelectList(_context.databases.Where(d => d.id_db.ToString() == dbid), "id_db", "db_name", db_grants.db_id);
            ViewData["db_grant_privs_id"] = new SelectList(_context.db_grant_privs, "id_db_grant_privs", "db_grant_priv_name", db_grants.db_grant_privs_id);
            ViewData["role_id"] = new SelectList(_context.roles, "id_role", "role_name", db_grants.role_id);
            return BadRequest("Произошла ошибка при обработке вашего запроса");
        }

        [HttpGet]
        public ActionResult RevokeGrant(Guid? id)
        {
            if (id == null || _context.db_grants == null)
            {
                return NotFound();
            }

            var db_grants = _context.db_grants
                .Include(d => d.databases)
                .Include(d => d.db_grant_privs)
                .Include(d => d.roles)
                .FirstOrDefault(m => m.id_db_grants == id);
			return PartialView("RevokeGrant", db_grants);
		}

        // POST: db_grants/Delete/5
        [HttpPost, ActionName("RevokeGrant")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var db_grants = await _context.db_grants.Include(d => d.databases)
                .Include(d => d.db_grant_privs)
                .Include(d => d.roles).FirstOrDefaultAsync(m => m.id_db_grants == id);
            if (db_grants != null)
            {
                var revoke = _context.Database.ExecuteSqlRaw($"Select update_revoke_db_typical_grants('{db_grants.id_db_grants}')") ;
                var listgrants = _context.Database.ExecuteSqlRaw($"select update_list_db_typical_grants()");
                var grants = _context.Database.ExecuteSqlRaw($"select update_grants_db_typical_grants()");
               
            }
            return Ok($"Права на {db_grants.db_grant_privs.db_grant_priv_name} в БД {db_grants.databases.db_name} отняты у роли {db_grants.roles.role_name} ");
        }

        private bool db_grantsExists(Guid id)
        {
          return (_context.db_grants?.Any(e => e.id_db_grants == id)).GetValueOrDefault();

        }


    }
}
