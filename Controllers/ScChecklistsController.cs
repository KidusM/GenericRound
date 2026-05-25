using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CSMS.Data;
using CSMS.Models;

namespace CSMS.Controllers
{
    public class ScChecklistsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScChecklistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ScChecklists
        [Route("Checklist")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScChecklist.ToListAsync());
        }

        // GET: ScChecklists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scChecklist = await _context.ScChecklist
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scChecklist == null)
            {
                return NotFound();
            }

            return View(scChecklist);
        }

        // GET: ScChecklists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScChecklists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,Time,Task,DetailTask,CbId,DetailTaskG1,GroupId")] ScChecklist scChecklist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scChecklist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scChecklist);
        }

        // GET: ScChecklists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scChecklist = await _context.ScChecklist.FindAsync(id);
            if (scChecklist == null)
            {
                return NotFound();
            }
            return View(scChecklist);
        }

        // POST: ScChecklists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,Time,Task,DetailTask,CbId,DetailTaskG1,GroupId")] ScChecklist scChecklist)
        {
            if (id != scChecklist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scChecklist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScChecklistExists(scChecklist.Id))
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
            return View(scChecklist);
        }

        // GET: ScChecklists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scChecklist = await _context.ScChecklist
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scChecklist == null)
            {
                return NotFound();
            }

            return View(scChecklist);
        }

        // POST: ScChecklists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scChecklist = await _context.ScChecklist.FindAsync(id);
            _context.ScChecklist.Remove(scChecklist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScChecklistExists(int id)
        {
            return _context.ScChecklist.Any(e => e.Id == id);
        }
    }
}
