using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CSMS.Data;
using CSMS.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.Json;
using CSMS.ViewComponents;
using Microsoft.AspNetCore.Authorization;

namespace CSMS.Controllers
{
    [Authorize]
    public class ScPricingDataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScPricingDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ScPricingData

        public async Task<IActionResult> Index()
        {

            var applicationDbContext = _context.scPricingDatas.Include(s => s.ItemCodeNavigation);
            return View(await applicationDbContext.ToListAsync());
        }



        // GET: ScPricingData/Details/5
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scPricingData = await _context.scPricingDatas
                .Include(s => s.ItemCodeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scPricingData == null)
            {
                return NotFound();
            }

            return View(scPricingData);
        }

        // GET: ScPricingData/Create

        public IActionResult Create()
        {
            var storeslist = _context.ScpricingStoress;
           
            var aaa = HttpContext.Session.GetString("_loggedInUserSysCode");
            var test = storeslist
                    .Where(a=>a.Syscode == HttpContext.Session.GetString("_loggedInUserSysCode"))
                    .Select(aa => new { aa.StoresId, aa.StoresName}).ToList(); 
                


            ViewData["ItemCode"] = new SelectList(_context.ScPricingItems, "ItemCode", "ItemName");
            ViewData["ScStoreId"] = new SelectList(test, "StoresId", "StoresName");
         
            return View();
        }

        // POST: ScPricingData/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DutyStation,City,SurveyMonth,SurveyYear,ItemCode,StoreId,Brand,Quantity,Unit,Price,Comments,BasicChargeUnit,BasicChargeRate,AdditionalChargeUnit,AdditionalChargeRate,OtherChargeUnit,OtherChargeRate,ThirdParty,Comprehensive,Collision,RegistrationFee,Syscode")] ScPricingData scPricingData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scPricingData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
                
            }

            var aaa = HttpContext.Session.GetString("_loggedInUserSysCode");
            var test = _context.ScpricingStoress
                    .Where(a => a.Syscode == HttpContext.Session.GetString("_loggedInUserSysCode"))
                    .Select(aa => new { aa.StoresId, aa.StoresName }).ToList();



            ViewData["ItemCode"] = new SelectList(_context.ScPricingItems, "ItemCode", "ItemName");
            ViewData["ScStoreId"] = new SelectList(test, "StoresId", "StoresName");

            //var storeslist = _context.ScStores
           //     .Include(s => s.SurveyStore);

            //var test = storeslist.Select(aa => new { aa.StoresId, aa.StoresName }).ToList();
          
            //ViewData["ItemCode"] = new SelectList(_context.ScPricingItems, "ItemCode", "ItemName");
           // ViewData["ScStoreId"] = new SelectList(test, "StoresId", "StoresName");
            // ModelState.Remove("ValidationStatus");
            return View(scPricingData);
        }

        // GET: ScPricingData/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scPricingData = await _context.scPricingDatas.FindAsync(id);
            if (scPricingData == null)
            {
                return NotFound();
            }
            ViewData["ItemCode"] = new SelectList(_context.ScPricingItems, "ItemCode", "ItemCode", scPricingData.ItemCode);
            return View(scPricingData);
        }

        // POST: ScPricingData/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DutyStation,City,SurveyMonth,SurveyYear,ItemCode,StoreId,Brand,Quantity,Unit,Price,Comments,BasicChargeUnit,BasicChargeRate,AdditionalChargeUnit,AdditionalChargeRate,OtherChargeUnit,OtherChargeRate,ThirdParty,Comprehensive,Collision,RegistrationFee,Syscode")] ScPricingData scPricingData)
        {
            if (id != scPricingData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scPricingData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScPricingDataExists(scPricingData.Id))
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
            ViewData["ItemCode"] = new SelectList(_context.ScPricingItems, "ItemCode", "ItemCode", scPricingData.ItemCode);
            return View(scPricingData);
        }

        // GET: ScPricingData/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scPricingData = await _context.scPricingDatas
                .Include(s => s.ItemCodeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scPricingData == null)
            {
                return NotFound();
            }

            return View(scPricingData);
        }

        // POST: ScPricingData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scPricingData = await _context.scPricingDatas.FindAsync(id);
            _context.scPricingDatas.Remove(scPricingData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScPricingDataExists(int id)
        {
            return _context.scPricingDatas.Any(e => e.Id == id);
        }

        [HttpPost]
        
        
        
        
        
        
        public JsonResult setCategory(string itemcode)
        {
            if (itemcode != "CHOOSE ITEM")
            {
                var categoryRecord = (from c in _context.ScPricingItems
                                      where c.ItemCode == itemcode
                                      select c).FirstOrDefault<ScPricingItem>();

                var category = categoryRecord.ItemCategory.ToString();
                var description = categoryRecord.ItemDescription.ToString();
                var defaultUnit = categoryRecord.DefaultUnit.ToString();
                var itemCode = categoryRecord.ItemCode.ToString();

                HttpContext.Session.SetString("_selectedItemCode", itemCode);
              
                var a = new ItemInfo
                {
                    categoryName = category,
                    DescriptionText = description,
                    Default_Unit = defaultUnit,
                    ItemCodee = itemCode
                };
              
                return Json(a);
            }
            else
            { 
                var b = new ItemInfo 
                { 
                   categoryName = "",
                   DescriptionText = "",
                   Default_Unit = "",
                   ItemCodee = ""
                };
                return Json(b); 
            }
        }

        public class ItemInfo
        {
            public string categoryName { get; set; }
            public string DescriptionText { get; set; }
            public string Default_Unit { get; set; }
            public string ItemCodee { get; set; }

        }

        public IActionResult CallVC(string itemcode)
        {
            return ViewComponent("Selected", new { myItemCode = itemcode });
       }


        public class DataAndMore
        {
            public ItemInfo itemInfo { get; set; }
            public IEnumerable<ScPricingData> scPricingDataPartial { get; set; }
        }
    }
}
