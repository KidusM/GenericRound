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
    public class ScPricingQuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScPricingQuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ScPricingQuestions
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScPricingQuestions.ToListAsync());
        }

        // GET: ScPricingQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scPricingQuestions = await _context.ScPricingQuestions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scPricingQuestions == null)
            {
                return NotFound();
            }

            return View(scPricingQuestions);
        }

        // GET: ScPricingQuestions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScPricingQuestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DutyStation,City,SurveyMonth,SurveyYear,ItemCode,StoreId,Brand,Quantity,Unit,Price,Comments,BasicChargeUnit,BasicChargeRate,AdditionalChargeUnit,AdditionalChargeRate,OtherChargeUnit,OtherChargeRate,ThirdParty,Comprehensive,Collision,RegistrationFee,Syscode")] ScPricingQuestions scPricingQuestions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scPricingQuestions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scPricingQuestions);
        }

        // GET: ScPricingQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scPricingQuestions = await _context.ScPricingQuestions.FindAsync(id);
            if (scPricingQuestions == null)
            {
                return NotFound();
            }
            return View(scPricingQuestions);
        }

        // POST: ScPricingQuestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DutyStation,City,SurveyMonth,SurveyYear,ItemCode,StoreId,Brand,Quantity,Unit,Price,Comments,BasicChargeUnit,BasicChargeRate,AdditionalChargeUnit,AdditionalChargeRate,OtherChargeUnit,OtherChargeRate,ThirdParty,Comprehensive,Collision,RegistrationFee,Syscode")] ScPricingQuestions scPricingQuestions)
        {
            if (id != scPricingQuestions.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scPricingQuestions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScPricingQuestionsExists(scPricingQuestions.Id))
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
            return View(scPricingQuestions);
        }

        // GET: ScPricingQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scPricingQuestions = await _context.ScPricingQuestions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scPricingQuestions == null)
            {
                return NotFound();
            }

            return View(scPricingQuestions);
        }

        // POST: ScPricingQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scPricingQuestions = await _context.ScPricingQuestions.FindAsync(id);
            _context.ScPricingQuestions.Remove(scPricingQuestions);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScPricingQuestionsExists(int id)
        {
            return _context.ScPricingQuestions.Any(e => e.Id == id);
        }
    }
}
