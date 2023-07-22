using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APP_PG_USERS_ROLES_SERVICE.Models;

namespace APP_PG_USERS_ROLES_SERVICE.Controllers
{
    public class db_grantsController : Controller
    {
        private readonly DataContext _context;

        public db_grantsController(DataContext context)
        {
            _context = context;
        }

        // GET: db_grants
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.db_grants.Include(d => d.databases).Include(d => d.db_grant_privs).Include(d => d.roles);
            return View(await dataContext.ToListAsync());
        }

        // GET: db_grants/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.db_grants == null)
            {
                return NotFound();
            }

            var db_grants = await _context.db_grants
                .Include(d => d.databases)
                .Include(d => d.db_grant_privs)
                .Include(d => d.roles)
                .FirstOrDefaultAsync(m => m.id_db_grants == id);
            if (db_grants == null)
            {
                return NotFound();
            }

            return View(db_grants);
        }

        // GET: db_grants/Create
        public IActionResult Create()
        {
            ViewData["db_id"] = new SelectList(_context.databases, "id_db", "id_db");
            ViewData["db_grant_privs_id"] = new SelectList(_context.db_grant_privs, "id_db_grant_privs", "id_db_grant_privs");
            ViewData["role_id"] = new SelectList(_context.roles, "id_role", "id_role");
            return View();
        }

        // POST: db_grants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_db_grants,db_grant_privs_id,db_id,date_time_exec,is_success,role_id")] db_grants db_grants)
        {
            if (ModelState.IsValid)
            {
                db_grants.id_db_grants = Guid.NewGuid();
                _context.Add(db_grants);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["db_id"] = new SelectList(_context.databases, "id_db", "id_db", db_grants.db_id);
            ViewData["db_grant_privs_id"] = new SelectList(_context.db_grant_privs, "id_db_grant_privs", "id_db_grant_privs", db_grants.db_grant_privs_id);
            ViewData["role_id"] = new SelectList(_context.roles, "id_role", "id_role", db_grants.role_id);
            return View(db_grants);
        }

        // GET: db_grants/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.db_grants == null)
            {
                return NotFound();
            }

            var db_grants = await _context.db_grants.FindAsync(id);
            if (db_grants == null)
            {
                return NotFound();
            }
            ViewData["db_id"] = new SelectList(_context.databases, "id_db", "id_db", db_grants.db_id);
            ViewData["db_grant_privs_id"] = new SelectList(_context.db_grant_privs, "id_db_grant_privs", "id_db_grant_privs", db_grants.db_grant_privs_id);
            ViewData["role_id"] = new SelectList(_context.roles, "id_role", "id_role", db_grants.role_id);
            return View(db_grants);
        }

        // POST: db_grants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id_db_grants,db_grant_privs_id,db_id,date_time_exec,is_success,role_id")] db_grants db_grants)
        {
            if (id != db_grants.id_db_grants)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(db_grants);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!db_grantsExists(db_grants.id_db_grants))
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
            ViewData["db_id"] = new SelectList(_context.databases, "id_db", "id_db", db_grants.db_id);
            ViewData["db_grant_privs_id"] = new SelectList(_context.db_grant_privs, "id_db_grant_privs", "id_db_grant_privs", db_grants.db_grant_privs_id);
            ViewData["role_id"] = new SelectList(_context.roles, "id_role", "id_role", db_grants.role_id);
            return View(db_grants);
        }

        // GET: db_grants/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.db_grants == null)
            {
                return NotFound();
            }

            var db_grants = await _context.db_grants
                .Include(d => d.databases)
                .Include(d => d.db_grant_privs)
                .Include(d => d.roles)
                .FirstOrDefaultAsync(m => m.id_db_grants == id);
            if (db_grants == null)
            {
                return NotFound();
            }

            return View(db_grants);
        }

        // POST: db_grants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.db_grants == null)
            {
                return Problem("Entity set 'DataContext.db_grants'  is null.");
            }
            var db_grants = await _context.db_grants.FindAsync(id);
            if (db_grants != null)
            {
                _context.db_grants.Remove(db_grants);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool db_grantsExists(Guid id)
        {
          return (_context.db_grants?.Any(e => e.id_db_grants == id)).GetValueOrDefault();
        }
    }
}
