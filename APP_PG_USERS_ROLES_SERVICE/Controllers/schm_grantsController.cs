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
    public class schm_grantsController : Controller
    {
        private readonly DataContext _context;

        public schm_grantsController(DataContext context)
        {
            _context = context;
        }

        // GET: schm_grants
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.schm_grants.Include(s => s.roles).Include(s => s.schemas).Include(s => s.schm_grant_privs);
            return View(await dataContext.ToListAsync());
        }

        // GET: schm_grants/Details/5
        public async Task<IActionResult> Details(Guid? id)
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
            if (schm_grants == null)
            {
                return NotFound();
            }

            return View(schm_grants);
        }

        // GET: schm_grants/Create
        public IActionResult Create()
        {
            ViewData["role_id"] = new SelectList(_context.roles, "id_role", "id_role");
            ViewData["schm_id"] = new SelectList(_context.schemas, "id_schm", "id_schm");
            ViewData["schm_grant_privs_id"] = new SelectList(_context.schm_grant_privs, "id_schm_grant_privs", "id_schm_grant_privs");
            return View();
        }

        // POST: schm_grants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_schm_grants,schm_grant_privs_id,schm_id,date_time_exec,is_success,role_id")] schm_grants schm_grants)
        {
            if (ModelState.IsValid)
            {
                schm_grants.id_schm_grants = Guid.NewGuid();
                _context.Add(schm_grants);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["role_id"] = new SelectList(_context.roles, "id_role", "id_role", schm_grants.role_id);
            ViewData["schm_id"] = new SelectList(_context.schemas, "id_schm", "id_schm", schm_grants.schm_id);
            ViewData["schm_grant_privs_id"] = new SelectList(_context.schm_grant_privs, "id_schm_grant_privs", "id_schm_grant_privs", schm_grants.schm_grant_privs_id);
            return View(schm_grants);
        }

        // GET: schm_grants/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.schm_grants == null)
            {
                return NotFound();
            }

            var schm_grants = await _context.schm_grants.FindAsync(id);
            if (schm_grants == null)
            {
                return NotFound();
            }
            ViewData["role_id"] = new SelectList(_context.roles, "id_role", "id_role", schm_grants.role_id);
            ViewData["schm_id"] = new SelectList(_context.schemas, "id_schm", "id_schm", schm_grants.schm_id);
            ViewData["schm_grant_privs_id"] = new SelectList(_context.schm_grant_privs, "id_schm_grant_privs", "id_schm_grant_privs", schm_grants.schm_grant_privs_id);
            return View(schm_grants);
        }

        // POST: schm_grants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id_schm_grants,schm_grant_privs_id,schm_id,date_time_exec,is_success,role_id")] schm_grants schm_grants)
        {
            if (id != schm_grants.id_schm_grants)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schm_grants);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!schm_grantsExists(schm_grants.id_schm_grants))
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
            ViewData["role_id"] = new SelectList(_context.roles, "id_role", "id_role", schm_grants.role_id);
            ViewData["schm_id"] = new SelectList(_context.schemas, "id_schm", "id_schm", schm_grants.schm_id);
            ViewData["schm_grant_privs_id"] = new SelectList(_context.schm_grant_privs, "id_schm_grant_privs", "id_schm_grant_privs", schm_grants.schm_grant_privs_id);
            return View(schm_grants);
        }

        // GET: schm_grants/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
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
            if (schm_grants == null)
            {
                return NotFound();
            }

            return View(schm_grants);
        }

        // POST: schm_grants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.schm_grants == null)
            {
                return Problem("Entity set 'DataContext.schm_grants'  is null.");
            }
            var schm_grants = await _context.schm_grants.FindAsync(id);
            if (schm_grants != null)
            {
                _context.schm_grants.Remove(schm_grants);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool schm_grantsExists(Guid id)
        {
          return (_context.schm_grants?.Any(e => e.id_schm_grants == id)).GetValueOrDefault();
        }
    }
}
