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
    public class ItemStorePricesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemStorePricesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ItemStorePrices
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ItemStorePrices.Include(i => i.ItemCodeNavigation).Include(i => i.S);
            return View(await applicationDbContext.ToListAsync());
        }


        public ActionResult MainView()
        {
            return View(); //this is main page.We will display  "_AddMorePartialView" partial page on this main page
        }
        public ActionResult ItemStorePricePartialView()
        {
            //this  action page is support cal the partial page.
            //We will call this action by view page.This Action is return partial page
            ItemStorePrice model = new ItemStorePrice();
            return PartialView("_ItemStorePricePartialView", model);
            //^this is actual partical page we have 
            //create on this page in Home Controller as given below image
        }
        public ActionResult PostAddMore(ScPricingStores model)
        {
            //Here,Post addmore value from view page and get multiple values from view page
            return View();
        }



        // GET: ItemStorePrices/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemStorePrice = await _context.ItemStorePrices
                .Include(i => i.ItemCodeNavigation)
                .Include(i => i.S)
                .FirstOrDefaultAsync(m => m.ItemCode == id);
            if (itemStorePrice == null)
            {
                return NotFound();
            }

            return View(itemStorePrice);
        }

        // GET: ItemStorePrices/Create
        public IActionResult Create()
        {
            ViewData["ItemCode"] = new SelectList(_context.ScPricingItems, "ItemCode", "ItemCode");
            ViewData["ItemName"] = new SelectList(_context.ScPricingItems, "ItemCode", "ItemName");
            ViewData["Syscode"] = new SelectList(_context.ScpricingStoress, "Syscode", "Syscode");
            ViewData["StoreId"] = new SelectList(_context.ScpricingStoress, "StoresId", "StoresId");
            ViewData["StoreName"] = new SelectList(_context.ScpricingStoress, "StoresId", "StoresName");

            ViewData["ItemDescription"] = new SelectList(_context.ScPricingItems, "ItemDescription", "ItemDescription");

            return View();
        }

        // POST: ItemStorePrices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemCode,Syscode,StoreId,Brand,UnitUsed,RegularPrice,Comment,OrganicFlag,Observation,SaleDiscount,Quantity,CostLocal")] ItemStorePrice itemStorePrice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemStorePrice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemCode"] = new SelectList(_context.ScPricingItems, "ItemCode", "ItemCode", itemStorePrice.ItemCode);
            ViewData["Syscode"] = new SelectList(_context.ScpricingStoress, "Syscode", "Syscode", itemStorePrice.Syscode);
            return View(itemStorePrice);

        }

        // GET: ItemStorePrices/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemStorePrice = await _context.ItemStorePrices.FindAsync(id);
            if (itemStorePrice == null)
            {
                return NotFound();
            }
            ViewData["ItemCode"] = new SelectList(_context.ScPricingItems, "ItemCode", "ItemCode", itemStorePrice.ItemCode);
            ViewData["Syscode"] = new SelectList(_context.ScpricingStoress, "Syscode", "Syscode", itemStorePrice.Syscode);
            return View(itemStorePrice);
        }

        // POST: ItemStorePrices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ItemCode,Syscode,StoreId,Brand,UnitUsed,RegularPrice,Comment,OrganicFlag,Observation,SaleDiscount,Quantity,CostLocal")] ItemStorePrice itemStorePrice)
        {
            if (id != itemStorePrice.ItemCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemStorePrice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemStorePriceExists(itemStorePrice.ItemCode))
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
            ViewData["ItemCode"] = new SelectList(_context.ScPricingItems, "ItemCode", "ItemCode", itemStorePrice.ItemCode);
            ViewData["Syscode"] = new SelectList(_context.ScpricingStoress, "Syscode", "Syscode", itemStorePrice.Syscode);
            return View(itemStorePrice);
        }

        // GET: ItemStorePrices/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemStorePrice = await _context.ItemStorePrices
                .Include(i => i.ItemCodeNavigation)
                .Include(i => i.S)
                .FirstOrDefaultAsync(m => m.ItemCode == id);
            if (itemStorePrice == null)
            {
                return NotFound();
            }

            return View(itemStorePrice);
        }

        // POST: ItemStorePrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var itemStorePrice = await _context.ItemStorePrices.FindAsync(id);
            _context.ItemStorePrices.Remove(itemStorePrice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemStorePriceExists(string id)
        {
            return _context.ItemStorePrices.Any(e => e.ItemCode == id);
        }
    }
}
