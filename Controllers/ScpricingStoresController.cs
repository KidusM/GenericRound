using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CSMS.Data;
using CSMS.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using OfficeOpenXml;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace CSMS.Controllers
{
    [Authorize]
    public class ScPricingStoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScPricingStoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ScPricingStores
        public async Task<IActionResult> Index()
        {
            string test1 = HttpContext.Session.GetString("_loggedInUserDSID");
           
            //test1 = "USA008";
            //var test = await _context.ScpricingStoress.Select(x => x.DsId == test1).ToListAsync());

            var DSstores = from m in _context.ScpricingStoress.OrderBy(x=>x.StoresId)
                         select m;

            if (!String.IsNullOrEmpty(test1))
            {
                DSstores = DSstores.Where(s => s.DsId == test1);
            }
            return View(await DSstores.ToListAsync());
            
          
           
            // return View(await _context.ScpricingStoress.ToListAsync());
        }

        // GET: ScPricingStores/Details/5
        public async Task<IActionResult> Details(string id, string syscode)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scPricingStores = await _context.ScpricingStoress
                .FirstOrDefaultAsync(m => m.Syscode == syscode && m.StoresId == id);
            if (scPricingStores == null)
            {
                return NotFound();
            }

            return View(scPricingStores);
        }

        // GET: ScPricingStores/Create
        public IActionResult Create()
        {
            string test1 = HttpContext.Session.GetString("_loggedInUserDSID");
            var DSstores = from m in _context.ScpricingStoress
                           select m;

            if (!String.IsNullOrEmpty(test1))
            {
                DSstores = DSstores.Where(s => s.DsId == test1);
                HttpContext.Session.SetString("_outletNumber", DSstores.Count().ToString());
                var test = HttpContext.Session.GetString("_outletNumber");
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DsId,Syscode,StoresId,ActualYear,ActualMonth,StoresName,StoresDescription,Exclude,ExRemarks,Internet,StoreAddress,WebAddress,GpsCoordinates,OutletTypeId")] ScPricingStores scPricingStores)
        {


            if (ModelState.IsValid)
            {
                _context.Add(scPricingStores);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Create));
            }
            return View(scPricingStores);
        }

        // GET: ScPricingStores/Edit/5
        public async Task<IActionResult> Edit(string syscode, string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scPricingStores = await _context.ScpricingStoress.FindAsync(syscode, id);
            if (scPricingStores == null)
            {
                return NotFound();
            }
            return View(scPricingStores);
        }

        // POST: ScPricingStores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string syscode, [Bind("DsId,Syscode,StoresId,ActualYear,ActualMonth,StoresName,StoresDescription,Exclude,ExRemarks,Internet,StoreAddress,WebAddress,GpsCoordinates,OutletTypeId")] ScPricingStores scPricingStores)
        {
            if (syscode != scPricingStores.Syscode || id != scPricingStores.StoresId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scPricingStores);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScPricingStoresExists(scPricingStores.Syscode, scPricingStores.StoresId))
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
            return View(scPricingStores);
        }

        // GET: ScPricingStores/Delete/5
        public async Task<IActionResult> Delete(string syscode, string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scPricingStores = await _context.ScpricingStoress
                .FirstOrDefaultAsync(m => m.Syscode == syscode && m.StoresId == id);
            if (scPricingStores == null)
            {
                return NotFound();
            }

            return View(scPricingStores);
        }

        // POST: ScPricingStores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string syscode, string id)
        {
            var scPricingStores = await _context.ScpricingStoress.FindAsync(syscode, id);
            _context.ScpricingStoress.Remove(scPricingStores);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScPricingStoresExists(string syscode, string id)
        {
            return _context.ScpricingStoress.Any(e => e.Syscode == syscode && e.StoresId == id);
        }




        //Method to Import stores in excel format
        public async Task<ActionResult> Import_xls(IFormFile xlsfile)
        {
            var list = new List<ScPricingStores>();
            using (var stream = new MemoryStream())
            {
                await xlsfile.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    //var rowcount = worksheet.Dimension.Rows;
                    var rowcount = Convert.ToInt16((worksheet.Cells[1, 1].Value ?? "0").ToString().Trim()) + 4;
                    //string StrZero = "0";
                    for (int row = 5; row <= rowcount; row++)
                    {

                        var _year = Convert.ToInt16((HttpContext.Session.GetString("_loggedInUserSurveyYear") ?? "0").ToString().Trim());
                        var _monthNo = DateTime.ParseExact(HttpContext.Session.GetString("_loggedInUserSurveyMonth"), "MMMM", CultureInfo.CurrentCulture).Month;
                        var _month = Convert.ToByte(_monthNo);
                        var _Name = (worksheet.Cells[row, 3].Value ?? string.Empty).ToString().Trim();
                        var _Address = (worksheet.Cells[row, 4].Value ?? string.Empty).ToString().Trim();
                        var _Web = (worksheet.Cells[row, 5].Value ?? string.Empty).ToString().Trim();
                        var _dsid = HttpContext.Session.GetString("_loggedInUserDSID").ToString().Trim();
                        var _Syscode = HttpContext.Session.GetString("_loggedInUserSysCode").ToString().Trim();
                        var _OutletId = (Convert.ToInt32(HttpContext.Session.GetString("_outletNumber")) + Convert.ToInt32(worksheet.Cells[row, 2].Value ?? "0")).ToString().Trim(); 


                        list.Add(new ScPricingStores
                        {
                            ActualYear = _year,
                            ActualMonth = _month,
                            StoresId = _OutletId,
                            StoresName = _Name,
                            StoreAddress = _Address,
                            WebAddress = _Address,
                            DsId = _dsid,
                            Syscode = _Syscode,
                        });
                    }
                    foreach (ScPricingStores scps in list)
                    {
                        _context.ScpricingStoress.Add(scps);
                    }

                    _context.SaveChanges();

                }

            }
            return View("Create");
        }

    }
}
