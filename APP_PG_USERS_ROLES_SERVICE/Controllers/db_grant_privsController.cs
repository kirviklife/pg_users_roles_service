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
    public class db_grant_privsController : Controller
    {
        private readonly DataContext _context;

        public db_grant_privsController(DataContext context)
        {
            _context = context;
        }

        // GET: db_grant_privs
        public async Task<IActionResult> Index()
        {
              return _context.db_grant_privs != null ? 
                          View(await _context.db_grant_privs.ToListAsync()) :
                          Problem("Entity set 'DataContext.db_grant_privs'  is null.");
        }

        // GET: db_grant_privs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.db_grant_privs == null)
            {
                return NotFound();
            }

            var db_grant_privs = await _context.db_grant_privs
                .FirstOrDefaultAsync(m => m.id_db_grant_privs == id);
            if (db_grant_privs == null)
            {
                return NotFound();
            }

            return View(db_grant_privs);
        }

        // GET: db_grant_privs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: db_grant_privs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_db_grant_privs,db_grant_priv_name")] db_grant_privs db_grant_privs)
        {
            if (ModelState.IsValid)
            {
                db_grant_privs.id_db_grant_privs = Guid.NewGuid();
                _context.Add(db_grant_privs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(db_grant_privs);
        }

        // GET: db_grant_privs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.db_grant_privs == null)
            {
                return NotFound();
            }

            var db_grant_privs = await _context.db_grant_privs.FindAsync(id);
            if (db_grant_privs == null)
            {
                return NotFound();
            }
            return View(db_grant_privs);
        }

        // POST: db_grant_privs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id_db_grant_privs,db_grant_priv_name")] db_grant_privs db_grant_privs)
        {
            if (id != db_grant_privs.id_db_grant_privs)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(db_grant_privs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!db_grant_privsExists(db_grant_privs.id_db_grant_privs))
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
            return View(db_grant_privs);
        }

        // GET: db_grant_privs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.db_grant_privs == null)
            {
                return NotFound();
            }

            var db_grant_privs = await _context.db_grant_privs
                .FirstOrDefaultAsync(m => m.id_db_grant_privs == id);
            if (db_grant_privs == null)
            {
                return NotFound();
            }

            return View(db_grant_privs);
        }

        // POST: db_grant_privs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.db_grant_privs == null)
            {
                return Problem("Entity set 'DataContext.db_grant_privs'  is null.");
            }
            var db_grant_privs = await _context.db_grant_privs.FindAsync(id);
            if (db_grant_privs != null)
            {
                _context.db_grant_privs.Remove(db_grant_privs);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool db_grant_privsExists(Guid id)
        {
          return (_context.db_grant_privs?.Any(e => e.id_db_grant_privs == id)).GetValueOrDefault();
        }
    }
}
