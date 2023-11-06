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
	public class srv_roles_relationsController : Controller
    {
        private readonly DataContext _context;

        public srv_roles_relationsController(DataContext context)
        {
            _context = context;
        }

        // GET: srv_roles_relations
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.srv_roles_relations.Include(s => s.roles).Include(s => s.servers);
            return View(await dataContext.ToListAsync());
        }

        // GET: srv_roles_relations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.srv_roles_relations == null)
            {
                return NotFound();
            }

            var srv_roles_relations = await _context.srv_roles_relations
                .Include(s => s.roles)
                .Include(s => s.servers)
                .FirstOrDefaultAsync(m => m.id_srv_role == id);
            if (srv_roles_relations == null)
            {
                return NotFound();
            }

            return View(srv_roles_relations);
        }

        // GET: srv_roles_relations/Create
        public IActionResult Create()
        {
            ViewData["role_id"] = new SelectList(_context.roles, "id_role", "role_name");
            ViewData["srv_id"] = new SelectList(_context.servers, "id_srv", "srv_name");
            return View();
        }

        // POST: srv_roles_relations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_srv_role,oid_roles,srv_id,role_id")] srv_roles_relations srv_roles_relations)
        {
            if (ModelState.IsValid)
            {
                srv_roles_relations.id_srv_role = Guid.NewGuid();
                _context.Add(srv_roles_relations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["role_id"] = new SelectList(_context.roles, "id_role", "role_name", srv_roles_relations.role_id);
            ViewData["srv_id"] = new SelectList(_context.servers, "id_srv", "srv_name", srv_roles_relations.srv_id);
            return View(srv_roles_relations);
        }

        // GET: srv_roles_relations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.srv_roles_relations == null)
            {
                return NotFound();
            }

            var srv_roles_relations = await _context.srv_roles_relations.FindAsync(id);
            if (srv_roles_relations == null)
            {
                return NotFound();
            }
            ViewData["role_id"] = new SelectList(_context.roles, "id_role", "role_name", srv_roles_relations.role_id);
            ViewData["srv_id"] = new SelectList(_context.servers, "id_srv", "srv_name", srv_roles_relations.srv_id);
            return View(srv_roles_relations);
        }

        // POST: srv_roles_relations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id_srv_role,oid_roles,srv_id,role_id")] srv_roles_relations srv_roles_relations)
        {
            if (id != srv_roles_relations.id_srv_role)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(srv_roles_relations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!srv_roles_relationsExists(srv_roles_relations.id_srv_role))
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
            ViewData["role_id"] = new SelectList(_context.roles, "id_role", "role_name", srv_roles_relations.role_id);
            ViewData["srv_id"] = new SelectList(_context.servers, "id_srv", "srv_name", srv_roles_relations.srv_id);
            return View(srv_roles_relations);
        }

        // GET: srv_roles_relations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.srv_roles_relations == null)
            {
                return NotFound();
            }

            var srv_roles_relations = await _context.srv_roles_relations
                .Include(s => s.roles)
                .Include(s => s.servers)
                .FirstOrDefaultAsync(m => m.id_srv_role == id);
            if (srv_roles_relations == null)
            {
                return NotFound();
            }

            return View(srv_roles_relations);
        }

        // POST: srv_roles_relations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.srv_roles_relations == null)
            {
                return Problem("Entity set 'DataContext.srv_roles_relations'  is null.");
            }
            var srv_roles_relations = await _context.srv_roles_relations.FindAsync(id);
            if (srv_roles_relations != null)
            {
                _context.srv_roles_relations.Remove(srv_roles_relations);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool srv_roles_relationsExists(Guid id)
        {
          return (_context.srv_roles_relations?.Any(e => e.id_srv_role == id)).GetValueOrDefault();
        }
    }
}
