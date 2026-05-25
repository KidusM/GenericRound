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
    public class IccdvwTmSchHdsController : Controller
    {
        private readonly IDMSDbContext _context;

        public IccdvwTmSchHdsController(IDMSDbContext context)
        {
            _context = context;
        }

        // GET: IccdvwTmSchHds
        public async Task<IActionResult> Index()
        {
            //using (var ctx = new IDMSDbContext())
            //{
            //    var studentName = ctx.IccdvwTmSchHds.SqlQuery("Select * from Courses").ToList();
            //}
            return View(await _context.IccdvwTmSchHds.ToListAsync());
        }

        //// GET: IccdvwTmSchHds/Details/5
        //public async Task<IActionResult> Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var iccdvwTmSchHd = await _context.IccdvwTmSchHds
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (iccdvwTmSchHd == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(iccdvwTmSchHd);
        //}

        //// GET: IccdvwTmSchHds/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: IccdvwTmSchHds/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Syscode,DsId,DsName,CountryName,ActualYear,ActualMonth,ProposedYear,ProposedMonth,DateWebStart,DateWebEnd,CurrencyId,ExRate,ActualDate,CountryId,SurveyType,FsPositions")] IccdvwTmSchHd iccdvwTmSchHd)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        iccdvwTmSchHd.Id = Guid.NewGuid();
        //        _context.Add(iccdvwTmSchHd);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(iccdvwTmSchHd);
        //}

        //// GET: IccdvwTmSchHds/Edit/5
        //public async Task<IActionResult> Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var iccdvwTmSchHd = await _context.IccdvwTmSchHds.FindAsync(id);
        //    if (iccdvwTmSchHd == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(iccdvwTmSchHd);
        //}

        //// POST: IccdvwTmSchHds/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(Guid id, [Bind("Id,Syscode,DsId,DsName,CountryName,ActualYear,ActualMonth,ProposedYear,ProposedMonth,DateWebStart,DateWebEnd,CurrencyId,ExRate,ActualDate,CountryId,SurveyType,FsPositions")] IccdvwTmSchHd iccdvwTmSchHd)
        //{
        //    if (id != iccdvwTmSchHd.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(iccdvwTmSchHd);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!IccdvwTmSchHdExists(iccdvwTmSchHd.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(iccdvwTmSchHd);
        //}

        //// GET: IccdvwTmSchHds/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var iccdvwTmSchHd = await _context.IccdvwTmSchHds
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (iccdvwTmSchHd == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(iccdvwTmSchHd);
        //}

        //// POST: IccdvwTmSchHds/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    var iccdvwTmSchHd = await _context.IccdvwTmSchHds.FindAsync(id);
        //    _context.IccdvwTmSchHds.Remove(iccdvwTmSchHd);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool IccdvwTmSchHdExists(Guid id)
        //{
        //    return _context.IccdvwTmSchHds.Any(e => e.Id == id);
        //}
    }
}
