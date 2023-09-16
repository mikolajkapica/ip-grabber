using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ip_grabber.Models;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace ip_grabber.Controllers
{

    public class IpEntriesController : Controller
    {
        private readonly IpEntryDbContext _context;

        public IpEntriesController(IpEntryDbContext context)
        {
            _context = context;
        }

        // GET: IpEntries
        public async Task<IActionResult> Index()
        {
              return _context.IpEntries != null ? 
                          View(await _context.IpEntries.ToListAsync()) :
                          Problem("Entity set 'IpEntryDbContext.IpEntries'  is null.");
        }

        // GET: IpEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.IpEntries == null)
            {
                return NotFound();
            }

            var ipEntry = await _context.IpEntries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ipEntry == null)
            {
                return NotFound();
            }

            return View(ipEntry);
        }

        // GET: IpEntries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IpEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Link,Data")] IpEntry ipEntry)
        {
			string api = "http://ipwho.is/?fields=ip,country,city,flag.emoji";
            var response = await new System.Net.Http.HttpClient().GetAsync(api);
            var content = await response.Content.ReadAsStringAsync();
            dynamic ipInfo = JObject.Parse(content);
            string ip = ipInfo.ip;
            string city = ipInfo.city;
            string country = ipInfo.country;

            ipEntry.Link = ip;
            ipEntry.Data = $"{country}, {city}";

			_context.Add(ipEntry);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));

            /*
            if (ModelState.IsValid)
            {
                _context.Add(ipEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ipEntry);
            */
        }

        // GET: IpEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.IpEntries == null)
            {
                return NotFound();
            }

            var ipEntry = await _context.IpEntries.FindAsync(id);
            if (ipEntry == null)
            {
                return NotFound();
            }
            return View(ipEntry);
        }

        // POST: IpEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Link,Data")] IpEntry ipEntry)
        {
            if (id != ipEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ipEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IpEntryExists(ipEntry.Id))
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
            return View(ipEntry);
        }

        // GET: IpEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.IpEntries == null)
            {
                return NotFound();
            }

            var ipEntry = await _context.IpEntries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ipEntry == null)
            {
                return NotFound();
            }

            return View(ipEntry);
        }

        // POST: IpEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.IpEntries == null)
            {
                return Problem("Entity set 'IpEntryDbContext.IpEntries'  is null.");
            }
            var ipEntry = await _context.IpEntries.FindAsync(id);
            if (ipEntry != null)
            {
                _context.IpEntries.Remove(ipEntry);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: IpEntries/Delete/5
        [HttpPost, ActionName("DeleteAll")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAll()
        {
            if (_context.IpEntries == null)
            {
                return Problem("Entity set 'IpEntryDbContext.IpEntries'  is null.");
            }
            // Delet all
            foreach (var item in _context.IpEntries)
            {
				_context.IpEntries.Remove(item);
			}

            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IpEntryExists(int id)
        {
          return (_context.IpEntries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
