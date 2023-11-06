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
	public class not_typical_grants_exec_logController : Controller
    {
        private readonly DataContext _context;

        public not_typical_grants_exec_logController(DataContext context)
        {
            _context = context;
        }

        // GET: not_typical_grants_exec_log
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.not_typical_grants_exec_log.Include(n => n.not_typical_grants_exec);
            return View(await dataContext.ToListAsync());
        }

        // GET: not_typical_grants_exec_log/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.not_typical_grants_exec_log == null)
            {
                return NotFound();
            }

            var not_typical_grants_exec_log = await _context.not_typical_grants_exec_log
                .Include(n => n.not_typical_grants_exec)
                .FirstOrDefaultAsync(m => m.id_not_typical_grants_exec_log == id);
            if (not_typical_grants_exec_log == null)
            {
                return NotFound();
            }

            return View(not_typical_grants_exec_log);
        }

        // GET: not_typical_grants_exec_log/Create
        public IActionResult Create()
        {
            ViewData["not_typical_grant_exec_id"] = new SelectList(_context.not_typical_grants_exec, "id_not_typical_grant_exec", "id_not_typical_grant_exec");
            return View();
        }

        // POST: not_typical_grants_exec_log/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_not_typical_grants_exec_log,not_typical_grant_exec_id,txt_error")] not_typical_grants_exec_log not_typical_grants_exec_log)
        {
            if (ModelState.IsValid)
            {
                not_typical_grants_exec_log.id_not_typical_grants_exec_log = Guid.NewGuid();
                _context.Add(not_typical_grants_exec_log);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["not_typical_grant_exec_id"] = new SelectList(_context.not_typical_grants_exec, "id_not_typical_grant_exec", "id_not_typical_grant_exec", not_typical_grants_exec_log.not_typical_grant_exec_id);
            return View(not_typical_grants_exec_log);
        }

        // GET: not_typical_grants_exec_log/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.not_typical_grants_exec_log == null)
            {
                return NotFound();
            }

            var not_typical_grants_exec_log = await _context.not_typical_grants_exec_log.FindAsync(id);
            if (not_typical_grants_exec_log == null)
            {
                return NotFound();
            }
            ViewData["not_typical_grant_exec_id"] = new SelectList(_context.not_typical_grants_exec, "id_not_typical_grant_exec", "id_not_typical_grant_exec", not_typical_grants_exec_log.not_typical_grant_exec_id);
            return View(not_typical_grants_exec_log);
        }

        // POST: not_typical_grants_exec_log/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id_not_typical_grants_exec_log,not_typical_grant_exec_id,txt_error")] not_typical_grants_exec_log not_typical_grants_exec_log)
        {
            if (id != not_typical_grants_exec_log.id_not_typical_grants_exec_log)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(not_typical_grants_exec_log);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!not_typical_grants_exec_logExists(not_typical_grants_exec_log.id_not_typical_grants_exec_log))
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
            ViewData["not_typical_grant_exec_id"] = new SelectList(_context.not_typical_grants_exec, "id_not_typical_grant_exec", "id_not_typical_grant_exec", not_typical_grants_exec_log.not_typical_grant_exec_id);
            return View(not_typical_grants_exec_log);
        }

        // GET: not_typical_grants_exec_log/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.not_typical_grants_exec_log == null)
            {
                return NotFound();
            }

            var not_typical_grants_exec_log = await _context.not_typical_grants_exec_log
                .Include(n => n.not_typical_grants_exec)
                .FirstOrDefaultAsync(m => m.id_not_typical_grants_exec_log == id);
            if (not_typical_grants_exec_log == null)
            {
                return NotFound();
            }

            return View(not_typical_grants_exec_log);
        }

        // POST: not_typical_grants_exec_log/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.not_typical_grants_exec_log == null)
            {
                return Problem("Entity set 'DataContext.not_typical_grants_exec_log'  is null.");
            }
            var not_typical_grants_exec_log = await _context.not_typical_grants_exec_log.FindAsync(id);
            if (not_typical_grants_exec_log != null)
            {
                _context.not_typical_grants_exec_log.Remove(not_typical_grants_exec_log);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool not_typical_grants_exec_logExists(Guid id)
        {
          return (_context.not_typical_grants_exec_log?.Any(e => e.id_not_typical_grants_exec_log == id)).GetValueOrDefault();
        }
    }
}
