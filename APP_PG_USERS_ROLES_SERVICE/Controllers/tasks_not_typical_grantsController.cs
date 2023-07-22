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
    public class tasks_not_typical_grantsController : Controller
    {
        private readonly DataContext _context;

        public tasks_not_typical_grantsController(DataContext context)
        {
            _context = context;
        }

        // GET: tasks_not_typical_grants
        public async Task<IActionResult> Index()
        {
              return _context.tasks_not_typical_grants != null ? 
                          View(await _context.tasks_not_typical_grants.ToListAsync()) :
                          Problem("Entity set 'DataContext.tasks_not_typical_grants'  is null.");
        }

        // GET: tasks_not_typical_grants/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.tasks_not_typical_grants == null)
            {
                return NotFound();
            }

            var tasks_not_typical_grants = await _context.tasks_not_typical_grants
                .FirstOrDefaultAsync(m => m.id_task == id);
            if (tasks_not_typical_grants == null)
            {
                return NotFound();
            }

            return View(tasks_not_typical_grants);
        }

        // GET: tasks_not_typical_grants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: tasks_not_typical_grants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_task,task_name,task_descr,task_script,count_params")] tasks_not_typical_grants tasks_not_typical_grants)
        {
            if (ModelState.IsValid)
            {
                tasks_not_typical_grants.id_task = Guid.NewGuid();
                _context.Add(tasks_not_typical_grants);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tasks_not_typical_grants);
        }

        // GET: tasks_not_typical_grants/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.tasks_not_typical_grants == null)
            {
                return NotFound();
            }

            var tasks_not_typical_grants = await _context.tasks_not_typical_grants.FindAsync(id);
            if (tasks_not_typical_grants == null)
            {
                return NotFound();
            }
            return View(tasks_not_typical_grants);
        }

        // POST: tasks_not_typical_grants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id_task,task_name,task_descr,task_script,count_params")] tasks_not_typical_grants tasks_not_typical_grants)
        {
            if (id != tasks_not_typical_grants.id_task)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tasks_not_typical_grants);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tasks_not_typical_grantsExists(tasks_not_typical_grants.id_task))
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
            return View(tasks_not_typical_grants);
        }

        // GET: tasks_not_typical_grants/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.tasks_not_typical_grants == null)
            {
                return NotFound();
            }

            var tasks_not_typical_grants = await _context.tasks_not_typical_grants
                .FirstOrDefaultAsync(m => m.id_task == id);
            if (tasks_not_typical_grants == null)
            {
                return NotFound();
            }

            return View(tasks_not_typical_grants);
        }

        // POST: tasks_not_typical_grants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.tasks_not_typical_grants == null)
            {
                return Problem("Entity set 'DataContext.tasks_not_typical_grants'  is null.");
            }
            var tasks_not_typical_grants = await _context.tasks_not_typical_grants.FindAsync(id);
            if (tasks_not_typical_grants != null)
            {
                _context.tasks_not_typical_grants.Remove(tasks_not_typical_grants);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tasks_not_typical_grantsExists(Guid id)
        {
          return (_context.tasks_not_typical_grants?.Any(e => e.id_task == id)).GetValueOrDefault();
        }
    }
}
