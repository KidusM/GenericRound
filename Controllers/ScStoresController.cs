using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CSMS.Data;
using CSMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace CSMS.Controllers
{
    [Authorize]
    public class ScStoresController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        public ScStoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ScStores
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScStores.ToListAsync());
        }

        // GET: ScStores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scStores = await _context.ScStores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scStores == null)
            {
                return NotFound();
            }

            return View(scStores);
        }

        // GET: ScStores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScStores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DsId,StoresId,StoresName,StoresDescription,StoreAddress,WebAddress,GpsCoordinates,OutletTypeId")] ScStores scStores)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scStores);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scStores);
        }

        // GET: ScStores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scStores = await _context.ScStores.FindAsync(id);
            if (scStores == null)
            {
                return NotFound();
            }
            return View(scStores);
        }

        // POST: ScStores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DsId,StoresId,StoresName,StoresDescription,StoreAddress,WebAddress,GpsCoordinates,OutletTypeId")] ScStores scStores)
        {
            if (id != scStores.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scStores);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScStoresExists(scStores.Id))
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
            return View(scStores);
        }

        // GET: ScStores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scStores = await _context.ScStores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scStores == null)
            {
                return NotFound();
            }

            return View(scStores);
        }

        // POST: ScStores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scStores = await _context.ScStores.FindAsync(id);
            _context.ScStores.Remove(scStores);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScStoresExists(int id)
        {
            return _context.ScStores.Any(e => e.Id == id);
        }
    }
}
