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
    public class ManagerDashboardViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public ManagerDashboardViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
                {
                var result = _context.Surveys.Include(s => s.Sm).Include(s => s.Ds).OrderBy(s=>s.Sm.LastName).ThenByDescending(s=>s.SurveyBegin);
                    return View(result);
                }

        }
    }
}