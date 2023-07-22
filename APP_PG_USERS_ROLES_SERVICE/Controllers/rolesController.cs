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
    public class rolesController : Controller
    {
        private readonly DataContext _context;

        public rolesController(DataContext context)
        {
            _context = context;
        }

        // GET: roles
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.roles.Include(r => r.servers);
            return View(await dataContext.ToListAsync());
        }

        // GET: roles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.roles == null)
            {
                return NotFound();
            }

            var roles = await _context.roles
                .Include(r => r.servers)
                .FirstOrDefaultAsync(m => m.id_role == id);
            if (roles == null)
            {
                return NotFound();
            }

            return View(roles);
        }

        // GET: roles/Create
        public IActionResult Create()
        {
            ViewData["srv_id"] = new SelectList(_context.servers, "id_srv", "id_srv");
            return View();
        }

        // POST: roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_role,role_name,role_pass,email,phone,fam,im,otch,srv_id,is_new_password,is_login,is_superuser,is_createdb,is_createrole,is_inherit,is_replication,valid_until")] roles roles)
        {
            ModelState.Remove("servers");
            if (ModelState.IsValid)
            {
                roles.id_role = Guid.NewGuid();
                _context.Add(roles);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["srv_id"] = new SelectList(_context.servers, "id_srv", "id_srv", roles.srv_id);
            return View(roles);
        }

        // GET: roles/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.roles == null)
            {
                return NotFound();
            }
            
            var roles = await _context.roles.FindAsync(id);
            if (roles == null)
            {
                return NotFound();
            }
            ViewData["srv_id"] = new SelectList(_context.servers, "id_srv", "id_srv", roles.srv_id);
            return View(roles);
        }

        // POST: roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id_role,role_name,role_pass,email,phone,fam,im,otch,srv_id,is_new_password,is_login,is_superuser,is_createdb,is_createrole,is_inherit,is_replication,valid_until")] roles roles)
        {
            if (id != roles.id_role)
            {
                return NotFound();
            }
            ModelState.Remove("servers");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!rolesExists(roles.id_role))
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
            ViewData["srv_id"] = new SelectList(_context.servers, "id_srv", "id_srv", roles.srv_id);
            return View(roles);
        }

        // GET: roles/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.roles == null)
            {
                return NotFound();
            }

            var roles = await _context.roles
                .Include(r => r.servers)
                .FirstOrDefaultAsync(m => m.id_role == id);
            if (roles == null)
            {
                return NotFound();
            }

            return View(roles);
        }

        // POST: roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.roles == null)
            {
                return Problem("Entity set 'DataContext.roles'  is null.");
            }
            var roles = await _context.roles.FindAsync(id);
            if (roles != null)
            {
                _context.roles.Remove(roles);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool rolesExists(Guid id)
        {
          return (_context.roles?.Any(e => e.id_role == id)).GetValueOrDefault();
        }
    }
}
