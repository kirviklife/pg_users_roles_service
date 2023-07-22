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
    public class schemasController : Controller
    {
        private readonly DataContext _context;

        public schemasController(DataContext context)
        {
            _context = context;
        }

        // GET: schemas
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.schemas.Include(s => s.databases);
            return View(await dataContext.ToListAsync());
        }

        // GET: schemas/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.schemas == null)
            {
                return NotFound();
            }

            var schemas = await _context.schemas
                .Include(s => s.databases)
                .FirstOrDefaultAsync(m => m.id_schm == id);
            if (schemas == null)
            {
                return NotFound();
            }

            return View(schemas);
        }

        // GET: schemas/Create
        public IActionResult Create()
        {
            ViewData["db_id"] = new SelectList(_context.databases, "id_db", "id_db");
            return View();
        }

        // POST: schemas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_schm,schm_name,db_id,oid_schm")] schemas schemas)
        {
            ModelState.Remove("databases");
            if (ModelState.IsValid)
            {
                schemas.id_schm = Guid.NewGuid();
                _context.Add(schemas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["db_id"] = new SelectList(_context.databases, "id_db", "id_db", schemas.db_id);
            return View(schemas);
        }

        // GET: schemas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.schemas == null)
            {
                return NotFound();
            }

            var schemas = await _context.schemas.FindAsync(id);
            if (schemas == null)
            {
                return NotFound();
            }
            ViewData["db_id"] = new SelectList(_context.databases, "id_db", "id_db", schemas.db_id);
            return View(schemas);
        }

        // POST: schemas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id_schm,schm_name,db_id,oid_schm")] schemas schemas)
        {
            if (id != schemas.id_schm)
            {
                return NotFound();
            }
            ModelState.Remove("databases");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schemas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!schemasExists(schemas.id_schm))
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
            ViewData["db_id"] = new SelectList(_context.databases, "id_db", "id_db", schemas.db_id);
            return View(schemas);
        }

        // GET: schemas/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.schemas == null)
            {
                return NotFound();
            }

            var schemas = await _context.schemas
                .Include(s => s.databases)
                .FirstOrDefaultAsync(m => m.id_schm == id);
            if (schemas == null)
            {
                return NotFound();
            }

            return View(schemas);
        }

        // POST: schemas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.schemas == null)
            {
                return Problem("Entity set 'DataContext.schemas'  is null.");
            }
            var schemas = await _context.schemas.FindAsync(id);
            if (schemas != null)
            {
                _context.schemas.Remove(schemas);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool schemasExists(Guid id)
        {
          return (_context.schemas?.Any(e => e.id_schm == id)).GetValueOrDefault();
        }
    }
}
