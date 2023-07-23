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
    public class serversController : Controller
    {
        private readonly DataContext _context;

        public serversController(DataContext context)
        {
            _context = context;
        }

        // GET: servers
        public async Task<IActionResult> Index()
        {
            ViewBag.Current = "Servers";
            return _context.view_servers_connect_checks != null ? 
                          View(await _context.view_servers_connect_checks.ToListAsync()) :
                          Problem("Entity set 'DataContext.servers'  is null.");
        }

        // GET: servers/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null || _context.servers == null)
            {
                return NotFound();
            }

            var servers = await _context.servers
                .FirstOrDefaultAsync(m => m.id_srv == id);
            if (servers == null)
            {
                return NotFound();
            }

            return View(servers);
        }

        // GET: servers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: servers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_srv,ipadd,srv_name,username,port_srv")] servers servers)
        {
            ModelState.Remove("id_srv");
            if (ModelState.IsValid)
            {
                _context.Add(servers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(servers);
        }

        // GET: servers/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null || _context.servers == null)
            {
                return NotFound();
            }

            var servers = await _context.servers.FindAsync(id);
            if (servers == null)
            {
                return NotFound();
            }
            return View(servers);
        }

        // POST: servers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id_srv,ipadd,srv_name,username,port_srv")] servers servers)
        {
            if (id != servers.id_srv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!serversExists(servers.id_srv))
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
            return View(servers);
        }

        // GET: servers/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null || _context.servers == null)
            {
                return NotFound();
            }

            var servers = await _context.servers
                .FirstOrDefaultAsync(m => m.id_srv == id);
            if (servers == null)
            {
                return NotFound();
            }

            return View(servers);
        }

        // POST: servers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.servers == null)
            {
                return Problem("Entity set 'DataContext.servers'  is null.");
            }
            var servers = await _context.servers.FindAsync(id);
            if (servers != null)
            {
                _context.servers.Remove(servers);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool serversExists(Guid id)
        {
          return (_context.servers?.Any(e => e.id_srv == id)).GetValueOrDefault();
        }
    }
}
