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
    public class not_typical_grants_execController : Controller
    {
        private readonly DataContext _context;

        public not_typical_grants_execController(DataContext context)
        {
            _context = context;
        }

        // GET: not_typical_grants_exec
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.not_typical_grants_exec.Include(n => n.not_typical_grants);
            return View(await dataContext.ToListAsync());
        }

        // GET: not_typical_grants_exec/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.not_typical_grants_exec == null)
            {
                return NotFound();
            }

            var not_typical_grants_exec = await _context.not_typical_grants_exec
                .Include(n => n.not_typical_grants)
                .FirstOrDefaultAsync(m => m.id_not_typical_grant_exec == id);
            if (not_typical_grants_exec == null)
            {
                return NotFound();
            }

            return View(not_typical_grants_exec);
        }

        // GET: not_typical_grants_exec/Create
        public IActionResult Create()
        {
            ViewData["not_typical_grant_id"] = new SelectList(_context.not_typical_grants, "id_not_typical_grant", "id_not_typical_grant");
            return View();
        }

        // POST: not_typical_grants_exec/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_not_typical_grant_exec,not_typical_grant_id,date_time_exec,is_success")] not_typical_grants_exec not_typical_grants_exec)
        {
            if (ModelState.IsValid)
            {
                not_typical_grants_exec.id_not_typical_grant_exec = Guid.NewGuid();
                _context.Add(not_typical_grants_exec);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["not_typical_grant_id"] = new SelectList(_context.not_typical_grants, "id_not_typical_grant", "id_not_typical_grant", not_typical_grants_exec.not_typical_grant_id);
            return View(not_typical_grants_exec);
        }

        // GET: not_typical_grants_exec/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.not_typical_grants_exec == null)
            {
                return NotFound();
            }

            var not_typical_grants_exec = await _context.not_typical_grants_exec.FindAsync(id);
            if (not_typical_grants_exec == null)
            {
                return NotFound();
            }
            ViewData["not_typical_grant_id"] = new SelectList(_context.not_typical_grants, "id_not_typical_grant", "id_not_typical_grant", not_typical_grants_exec.not_typical_grant_id);
            return View(not_typical_grants_exec);
        }

        // POST: not_typical_grants_exec/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id_not_typical_grant_exec,not_typical_grant_id,date_time_exec,is_success")] not_typical_grants_exec not_typical_grants_exec)
        {
            if (id != not_typical_grants_exec.id_not_typical_grant_exec)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(not_typical_grants_exec);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!not_typical_grants_execExists(not_typical_grants_exec.id_not_typical_grant_exec))
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
            ViewData["not_typical_grant_id"] = new SelectList(_context.not_typical_grants, "id_not_typical_grant", "id_not_typical_grant", not_typical_grants_exec.not_typical_grant_id);
            return View(not_typical_grants_exec);
        }

        // GET: not_typical_grants_exec/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.not_typical_grants_exec == null)
            {
                return NotFound();
            }

            var not_typical_grants_exec = await _context.not_typical_grants_exec
                .Include(n => n.not_typical_grants)
                .FirstOrDefaultAsync(m => m.id_not_typical_grant_exec == id);
            if (not_typical_grants_exec == null)
            {
                return NotFound();
            }

            return View(not_typical_grants_exec);
        }

        // POST: not_typical_grants_exec/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.not_typical_grants_exec == null)
            {
                return Problem("Entity set 'DataContext.not_typical_grants_exec'  is null.");
            }
            var not_typical_grants_exec = await _context.not_typical_grants_exec.FindAsync(id);
            if (not_typical_grants_exec != null)
            {
                _context.not_typical_grants_exec.Remove(not_typical_grants_exec);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool not_typical_grants_execExists(Guid id)
        {
          return (_context.not_typical_grants_exec?.Any(e => e.id_not_typical_grant_exec == id)).GetValueOrDefault();
        }
    }
}
