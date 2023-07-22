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
    public class databasesController : Controller
    {
        private readonly DataContext _context;

        public databasesController(DataContext context)
        {
            _context = context;
        }

        // GET: databases
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.databases.Include(d => d.servers);
            return View(await dataContext.ToListAsync());
        }

        // GET: databases/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.databases == null)
            {
                return NotFound();
            }

            var databases = await _context.databases
                .Include(d => d.servers)
                .FirstOrDefaultAsync(m => m.id_db == id);
            if (databases == null)
            {
                return NotFound();
            }

            return View(databases);
        }

        // GET: databases/Create
        public IActionResult Create()
        {
            ViewData["srv_id"] = new SelectList(_context.servers, "id_srv", "id_srv");
            return View();
        }

        // POST: databases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_db,db_name,srv_id,oid_db")] databases databases)
        {
            ModelState.Remove("servers");
            if (ModelState.IsValid)
            {
                databases.id_db = Guid.NewGuid();
                _context.Add(databases);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["srv_id"] = new SelectList(_context.servers, "id_srv", "id_srv", databases.srv_id);
            return View(databases);
        }

        // GET: databases/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.databases == null)
            {
                return NotFound();
            }

            var databases = await _context.databases.FindAsync(id);
            if (databases == null)
            {
                return NotFound();
            }
            ViewData["srv_id"] = new SelectList(_context.servers, "id_srv", "id_srv", databases.srv_id);
            return View(databases);
        }

        // POST: databases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id_db,db_name,srv_id,oid_db")] databases databases)
        {
            if (id != databases.id_db)
            {
                return NotFound();
            }
            ModelState.Remove("servers");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(databases);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!databasesExists(databases.id_db))
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
            ViewData["srv_id"] = new SelectList(_context.servers, "id_srv", "id_srv", databases.srv_id);
            return View(databases);
        }

        // GET: databases/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.databases == null)
            {
                return NotFound();
            }

            var databases = await _context.databases
                .Include(d => d.servers)
                .FirstOrDefaultAsync(m => m.id_db == id);
            if (databases == null)
            {
                return NotFound();
            }

            return View(databases);
        }

        // POST: databases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.databases == null)
            {
                return Problem("Entity set 'DataContext.databases'  is null.");
            }
            var databases = await _context.databases.FindAsync(id);
            if (databases != null)
            {
                _context.databases.Remove(databases);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool databasesExists(Guid id)
        {
          return (_context.databases?.Any(e => e.id_db == id)).GetValueOrDefault();
        }
    }
}
