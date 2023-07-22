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
    public class users_roles_relationController : Controller
    {
        private readonly DataContext _context;

        public users_roles_relationController(DataContext context)
        {
            _context = context;
        }

        // GET: users_roles_relation
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.users_roles_relation.Include(u => u.roles1).Include(u => u.roles2);
            return View(await dataContext.ToListAsync());
        }

        // GET: users_roles_relation/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.users_roles_relation == null)
            {
                return NotFound();
            }

            var users_roles_relation = await _context.users_roles_relation
                .Include(u => u.roles1)
                .Include(u => u.roles2)
                .FirstOrDefaultAsync(m => m.id_users_roles_relation == id);
            if (users_roles_relation == null)
            {
                return NotFound();
            }

            return View(users_roles_relation);
        }

        // GET: users_roles_relation/Create
        public IActionResult Create()
        {
            ViewData["from_role"] = new SelectList(_context.roles, "id_role", "id_role");
            ViewData["to_role"] = new SelectList(_context.roles, "id_role", "id_role");
            return View();
        }

        // POST: users_roles_relation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_users_roles_relation,from_role,to_role,date_time_excec")] users_roles_relation users_roles_relation)
        {
            ModelState.Remove("roles1");
            ModelState.Remove("roles2");
            if (ModelState.IsValid)
            {
                users_roles_relation.id_users_roles_relation = Guid.NewGuid();
                _context.Add(users_roles_relation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["from_role"] = new SelectList(_context.roles, "id_role", "id_role", users_roles_relation.from_role);
            ViewData["to_role"] = new SelectList(_context.roles, "id_role", "id_role", users_roles_relation.to_role);
            return View(users_roles_relation);
        }

        // GET: users_roles_relation/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.users_roles_relation == null)
            {
                return NotFound();
            }

            var users_roles_relation = await _context.users_roles_relation.FindAsync(id);
            if (users_roles_relation == null)
            {
                return NotFound();
            }
            ViewData["from_role"] = new SelectList(_context.roles, "id_role", "id_role", users_roles_relation.from_role);
            ViewData["to_role"] = new SelectList(_context.roles, "id_role", "id_role", users_roles_relation.to_role);
            return View(users_roles_relation);
        }

        // POST: users_roles_relation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id_users_roles_relation,from_role,to_role,date_time_excec")] users_roles_relation users_roles_relation)
        {
            if (id != users_roles_relation.id_users_roles_relation)
            {
                return NotFound();
            }
            ModelState.Remove("roles1");
            ModelState.Remove("roles2");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users_roles_relation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!users_roles_relationExists(users_roles_relation.id_users_roles_relation))
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
            ViewData["from_role"] = new SelectList(_context.roles, "id_role", "id_role", users_roles_relation.from_role);
            ViewData["to_role"] = new SelectList(_context.roles, "id_role", "id_role", users_roles_relation.to_role);
            return View(users_roles_relation);
        }

        // GET: users_roles_relation/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.users_roles_relation == null)
            {
                return NotFound();
            }

            var users_roles_relation = await _context.users_roles_relation
                .Include(u => u.roles1)
                .Include(u => u.roles2)
                .FirstOrDefaultAsync(m => m.id_users_roles_relation == id);
            if (users_roles_relation == null)
            {
                return NotFound();
            }

            return View(users_roles_relation);
        }

        // POST: users_roles_relation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.users_roles_relation == null)
            {
                return Problem("Entity set 'DataContext.users_roles_relation'  is null.");
            }
            var users_roles_relation = await _context.users_roles_relation.FindAsync(id);
            if (users_roles_relation != null)
            {
                _context.users_roles_relation.Remove(users_roles_relation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool users_roles_relationExists(Guid id)
        {
          return (_context.users_roles_relation?.Any(e => e.id_users_roles_relation == id)).GetValueOrDefault();
        }
    }
}
