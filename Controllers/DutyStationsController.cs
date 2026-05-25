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
    public class DutyStationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DutyStationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DutyStations
        public async Task<IActionResult> Index()
        {
            return View(await _context.DutyStations.ToListAsync());
        }

        // GET: DutyStations/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dutyStation = await _context.DutyStations
                .Include(s => s.Surveys)
                .FirstOrDefaultAsync(m => m.Dsid == id);
            if (dutyStation == null)
            {
                return NotFound();
            }

            return View(dutyStation);
        }

        // GET: DutyStations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DutyStations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Dsid,Country,City,Dsgroup")] DutyStation dutyStation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dutyStation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dutyStation);
        }

        // GET: DutyStations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dutyStation = await _context.DutyStations.FindAsync(id);
            if (dutyStation == null)
            {
                return NotFound();
            }
            return View(dutyStation);
        }

        // POST: DutyStations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Dsid,Country,City,Dsgroup")] DutyStation dutyStation)
        {
            if (id != dutyStation.Dsid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dutyStation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DutyStationExists(dutyStation.Dsid))
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
            return View(dutyStation);
        }

        // GET: DutyStations/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dutyStation = await _context.DutyStations
                .FirstOrDefaultAsync(m => m.Dsid == id);
            if (dutyStation == null)
            {
                return NotFound();
            }

            return View(dutyStation);
        }

        // POST: DutyStations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var dutyStation = await _context.DutyStations.FindAsync(id);
            _context.DutyStations.Remove(dutyStation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DutyStationExists(string id)
        {
            return _context.DutyStations.Any(e => e.Dsid == id);
        }
    }
}
