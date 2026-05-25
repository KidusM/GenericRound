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
    public class FocalPointDashboardViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public FocalPointDashboardViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public  IViewComponentResult Invoke(string id)
        {
            if (id != null && id!= "") 
            {
                {
                    var result = _context.Surveys.Include(s => s.Sm).Include(s => s.Ds)
                                           .Where(a => a.Smid == id);
                    return View(result);
                }
            
            }
            else
            {
                return View(null);
            }

        }
    }
}