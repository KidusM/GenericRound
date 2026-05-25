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
    public class IccdtTmSchHdsController : Controller
    {
        private readonly IDMSDbContext _context;

        public IccdtTmSchHdsController(IDMSDbContext context)
        {
            _context = context;
        }

        // GET: IccdtTmSchHds
        public async Task<IActionResult> Index()
        {
            var iDMSDbContext = _context.IccdtTmSchHds.Include(i => i.Ds);
            return View(await iDMSDbContext.ToListAsync());
        }

        // GET: IccdtTmSchHds/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iccdtTmSchHd = await _context.IccdtTmSchHds
                .Include(i => i.Ds)
                .FirstOrDefaultAsync(m => m.Syscode == id);
            if (iccdtTmSchHd == null)
            {
                return NotFound();
            }

            return View(iccdtTmSchHd);
        }

        // GET: IccdtTmSchHds/Create
        public IActionResult Create()
        {
            ViewData["DsId"] = new SelectList(_context.Set<IccdmDutystations>(), "DsId", "DsId");
            return View();
        }

        // POST: IccdtTmSchHds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Syscode,DsId,SurveyType,ProposedYear,ProposedMonth,ActualYear,ActualMonth,NoCcaq,LeadAgency,PersonnelStaff,Response,ResponseRate,DateVerified,DateUpdated,DateResults,DateImplemented,DateReported,Followup,FollowupComments,Cancelled,DataEntryFlag,VerifyFlag,UpdationFlag,DateCv,Salary,ExRate,IndicativeFees,HhMover,DateWebStart,DateWebEnd,FsPositions")] IccdtTmSchHd iccdtTmSchHd)
        {
            if (ModelState.IsValid)
            {
                _context.Add(iccdtTmSchHd);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DsId"] = new SelectList(_context.Set<IccdmDutystations>(), "DsId", "DsId", iccdtTmSchHd.DsId);
            return View(iccdtTmSchHd);
        }

        // GET: IccdtTmSchHds/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iccdtTmSchHd = await _context.IccdtTmSchHds.FindAsync(id);
            if (iccdtTmSchHd == null)
            {
                return NotFound();
            }
            ViewData["DsId"] = new SelectList(_context.Set<IccdmDutystations>(), "DsId", "DsId", iccdtTmSchHd.DsId);
            return View(iccdtTmSchHd);
        }

        // POST: IccdtTmSchHds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Syscode,DsId,SurveyType,ProposedYear,ProposedMonth,ActualYear,ActualMonth,NoCcaq,LeadAgency,PersonnelStaff,Response,ResponseRate,DateVerified,DateUpdated,DateResults,DateImplemented,DateReported,Followup,FollowupComments,Cancelled,DataEntryFlag,VerifyFlag,UpdationFlag,DateCv,Salary,ExRate,IndicativeFees,HhMover,DateWebStart,DateWebEnd,FsPositions")] IccdtTmSchHd iccdtTmSchHd)
        {
            if (id != iccdtTmSchHd.Syscode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(iccdtTmSchHd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IccdtTmSchHdExists(iccdtTmSchHd.Syscode))
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
            ViewData["DsId"] = new SelectList(_context.Set<IccdmDutystations>(), "DsId", "DsId", iccdtTmSchHd.DsId);
            return View(iccdtTmSchHd);
        }

        // GET: IccdtTmSchHds/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iccdtTmSchHd = await _context.IccdtTmSchHds
                .Include(i => i.Ds)
                .FirstOrDefaultAsync(m => m.Syscode == id);
            if (iccdtTmSchHd == null)
            {
                return NotFound();
            }

            return View(iccdtTmSchHd);
        }

        // POST: IccdtTmSchHds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var iccdtTmSchHd = await _context.IccdtTmSchHds.FindAsync(id);
            _context.IccdtTmSchHds.Remove(iccdtTmSchHd);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IccdtTmSchHdExists(string id)
        {
            return _context.IccdtTmSchHds.Any(e => e.Syscode == id);
        }
    }
}
