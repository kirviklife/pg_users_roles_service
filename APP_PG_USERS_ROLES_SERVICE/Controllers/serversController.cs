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
            //Подчеркивать пункт меню Сервера
            ViewBag.Current = "Servers";
            return _context.view_servers_connect_checks != null ? 
                          View(await _context.view_servers_connect_checks.ToListAsync()) :
                          Problem("Entity set 'DataContext.servers'  is null.");
        }

        [HttpGet]
        public async Task<ActionResult> GetServers(string SearchRelRole, int selectRel)
        {
            if (selectRel == 0)
            {
				var order = await _context.view_servers_connect_checks.Where(o => EF.Functions.Like(o.srv_name, "%" + SearchRelRole + "%") || EF.Functions.Like(o.ipadd, "%" + SearchRelRole + "%")).ToListAsync();
				return PartialView("GetServers", order);
			}
            else if (selectRel == 1)
            {
				var order = await _context.view_servers_connect_checks.Where(o => o.check == "OK" && (EF.Functions.Like(o.srv_name, "%" + SearchRelRole + "%") || EF.Functions.Like(o.ipadd, "%" + SearchRelRole + "%"))).ToListAsync();
				return PartialView("GetServers", order);
			}
			else if (selectRel == 2)
            {
				var order = await _context.view_servers_connect_checks.Where(o => o.check != "OK" && (EF.Functions.Like(o.srv_name, "%" + SearchRelRole + "%") || EF.Functions.Like(o.ipadd, "%" + SearchRelRole + "%"))).ToListAsync();
				return PartialView("GetServers", order);
			}
			else
            {
				var order = await _context.view_servers_connect_checks.Where(o => EF.Functions.Like(o.srv_name, "%" + SearchRelRole + "%") || EF.Functions.Like(o.ipadd, "%" + SearchRelRole + "%")).ToListAsync();
				return PartialView("GetServers", order);
			}

            
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
			ViewBag.Current = "Servers";
			return View();
        }

        // POST: servers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_srv,ipadd,srv_name,username,port_srv")] servers servers)
        {
			ViewBag.Current = "Servers";
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
			ViewBag.Current = "Servers";
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
			ViewBag.Current = "Servers";
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
			ViewBag.Current = "Servers";
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
