using CSMS.Data;
using CSMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMS.ViewComponents
{
    public class SelectedViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public SelectedViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string myItemCode)
        {

            {
                var result2 = _context.scPricingDatas.Include(s => s.ItemCodeNavigation)
                                       .Where(sys => sys.Syscode == HttpContext.Session.GetString("_loggedInUserSysCode"));
                return View(await result2.ToListAsync());
            }
            
            
        }
    }
}

