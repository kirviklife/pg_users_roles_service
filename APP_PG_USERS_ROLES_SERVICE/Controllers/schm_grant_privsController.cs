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
    public class schm_grant_privsController : Controller
    {
        private readonly DataContext _context;

        public schm_grant_privsController(DataContext context)
        {
            _context = context;
        }

        // GET: schm_grant_privs
        public async Task<IActionResult> Index()
        {
              return _context.schm_grant_privs != null ? 
                          View(await _context.schm_grant_privs.ToListAsync()) :
                          Problem("Entity set 'DataContext.schm_grant_privs'  is null.");
        }

        // GET: schm_grant_privs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.schm_grant_privs == null)
            {
                return NotFound();
            }

            var schm_grant_privs = await _context.schm_grant_privs
                .FirstOrDefaultAsync(m => m.id_schm_grant_privs == id);
            if (schm_grant_privs == null)
            {
                return NotFound();
            }

            return View(schm_grant_privs);
        }

        // GET: schm_grant_privs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: schm_grant_privs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_schm_grant_privs,schm_grant_priv_name")] schm_grant_privs schm_grant_privs)
        {
            if (ModelState.IsValid)
            {
                schm_grant_privs.id_schm_grant_privs = Guid.NewGuid();
                _context.Add(schm_grant_privs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(schm_grant_privs);
        }

        // GET: schm_grant_privs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.schm_grant_privs == null)
            {
                return NotFound();
            }

            var schm_grant_privs = await _context.schm_grant_privs.FindAsync(id);
            if (schm_grant_privs == null)
            {
                return NotFound();
            }
            return View(schm_grant_privs);
        }

        // POST: schm_grant_privs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id_schm_grant_privs,schm_grant_priv_name")] schm_grant_privs schm_grant_privs)
        {
            if (id != schm_grant_privs.id_schm_grant_privs)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schm_grant_privs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!schm_grant_privsExists(schm_grant_privs.id_schm_grant_privs))
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
            return View(schm_grant_privs);
        }

        // GET: schm_grant_privs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.schm_grant_privs == null)
            {
                return NotFound();
            }

            var schm_grant_privs = await _context.schm_grant_privs
                .FirstOrDefaultAsync(m => m.id_schm_grant_privs == id);
            if (schm_grant_privs == null)
            {
                return NotFound();
            }

            return View(schm_grant_privs);
        }

        // POST: schm_grant_privs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.schm_grant_privs == null)
            {
                return Problem("Entity set 'DataContext.schm_grant_privs'  is null.");
            }
            var schm_grant_privs = await _context.schm_grant_privs.FindAsync(id);
            if (schm_grant_privs != null)
            {
                _context.schm_grant_privs.Remove(schm_grant_privs);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool schm_grant_privsExists(Guid id)
        {
          return (_context.schm_grant_privs?.Any(e => e.id_schm_grant_privs == id)).GetValueOrDefault();
        }
    }
}
