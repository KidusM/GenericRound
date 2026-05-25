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
    public class FaqsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FaqsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Faqs
        [Route("FAQs")]
        public async Task<IActionResult> Index()
        {
            
            return View(await _context.Faqs.OrderBy(x => x.Qtype).ThenBy(x => x.ID).ToListAsync());
        }

        // GET: Faqs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faq = await _context.Faqs
                .FirstOrDefaultAsync(m => m.Pk == id);
            if (faq == null)
            {
                return NotFound();
            }

            return View(faq);
        }

        // GET: Faqs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Faqs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Create([Bind("ID,Question,Answer,Category,Qtype,GroupId")] Faq faq)
        {
            // Generate PK before checking ModelState
            int nextPk = 1;

            if (await _context.Faqs.AnyAsync())
            {
                var pks = await _context.Faqs
                    .Select(x => x.Pk)
                    .ToListAsync();

                nextPk = pks
                    .Select(x => int.Parse(x))
                    .Max() + 1;
            }

            faq.Pk = nextPk.ToString();

            // Remove old validation error for missing Pk
            ModelState.Remove("Pk");

            if (ModelState.IsValid)
            {
                _context.Add(faq);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(faq);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Pk,Id,Question,Answer,Category,Qtype,GroupId")] Faq faq)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(faq);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(faq);
        //}



        // GET: Faqs/Edit/5
        public async Task<IActionResult> Edit(string pk)
        {
            if (pk == null)
            {
                return NotFound();
            }

            var faq = await _context.Faqs.FindAsync(pk);
            if (faq == null)
            {
                return NotFound();
            }
            return View(faq);
        }

        // POST: Faqs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string pk, [Bind("Pk,Id,Question,Answer,Category,Qtype,GroupId")] Faq faq)
        public async Task<IActionResult> Edit(string pk, [Bind("Pk,ID,Question,Answer,Category,Qtype,GroupId")] Faq faq)
        {
            if (pk != faq.Pk)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faq);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaqExists(faq.Pk))
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
            return View(faq);
        }

        // GET: Faqs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faq = await _context.Faqs
                .FirstOrDefaultAsync(m => m.Pk == id);
            if (faq == null)
            {
                return NotFound();
            }

            return View(faq);
        }

        // POST: Faqs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var faq = await _context.Faqs.FindAsync(id);
            if (faq != null)
            {
                _context.Faqs.Remove(faq);
            }
            //_context.Faqs.Remove(faq);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FaqExists(string id)
        {
            return _context.Faqs.Any(e => e.Pk == id);
        }
    }
}
