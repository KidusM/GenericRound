using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CSMS.Data;
using CSMS.Models;
using System.Globalization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Collections;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace CSMS.Controllers
{
    [Authorize]
    //[Authorize(Roles = "Survey Coordinator, ICSC Focal Person, Adminstrator")]
    public class SurveysController : Controller
{
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private string CoordinatorRolecode;
        private string FocalPersonRolecode;

        public SurveysController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager, RoleManager<IdentityRole> rolemanager)
        {
            _context = context;
            _roleManager = rolemanager;
            _userManager = usermanager;

            CoordinatorRolecode = GetRoleID("Survey Coordinator");

            FocalPersonRolecode = GetRoleID("ICSC Survey Manager");

        }

        // GET: Surveys
        // [Authorize(Roles = "Survey Coordinator, ICSC Focal Personr, Adminstrator")]

        [Authorize(Roles = "Administrator,ICSC Focal Person, ICSC Survey Manager")]
        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = _context.Surveys.Include(s => s.Ds).Include(s => s.Sc).Include(s => s.Sm).Where(x=>x.SurveyBegin <= DateTime.Now).Where(xx=>xx.SurveyEnd>= DateTime.Now);
            var applicationDbContext = _context.Surveys.Include(s => s.Ds).Include(s => s.Sc).Include(s => s.Sm)
                .OrderByDescending(s=>s.SurveyBegin).ThenBy(s=>s.Ds.Country);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Surveys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var survey = await _context.Surveys
                .Include(s => s.Ds)
                .Include(s => s.Sc)
                .Include(s => s.Sm)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (survey == null)
            {
                return NotFound();
            }



            ViewData["SurveyDsid"] = survey.Ds.Country.ToString() + " - " + survey.Ds.City.ToString();

            
            return View(survey);
        }

        // GET: Surveys/Create
        //    [Authorize(Roles = "Survey Coordinator, ICSC Focal Personr, Adminstrator")]
        [Authorize(Roles = "Administrator,ICSC Focal Person, ICSC Survey Manager")]
        public IActionResult Create()
        {


            ViewData["SurveyTypeName"] = new SelectList(GetSurveyType(), "shortName", "longName");
            ViewData["Dsid"] = new SelectList((from s in _context.DutyStations.ToList()
                    select new { Dsid = s.Dsid, CountryCity = (s.Country + " - " + s.City + " - " + s.Dsid).Trim() }), "Dsid","CountryCity");

            ViewData["Syscode"] = new SelectList((from a in _context.Surveys.ToList()
                                                  select new { syscode = a.Dsid.Trim() + a.SurveyMonth.Trim() + a.SurveyYear.Trim() + a.SurveyType.Trim() }), "Syscode", "Syscode");

            ViewData["Scid"] = new SelectList((from s in _context.applicationUsers.ToList()
                                               select new { Scid = s.Id, FullName = s.FirstName + " " + s.LastName }), "Scid", "FullName");
            ViewData["Smid"] = new SelectList((from s in _context.applicationUsers.ToList()
                                               select new { Smid = s.Id, FullName = s.FirstName + " " + s.LastName }), "Smid", "FullName");


            
            List<string> monthnames = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList();
            IEnumerable<SelectListItem> monthlist = monthnames.Select(m => new SelectListItem { Text = m, Value = m.ToUpper() });
            ViewData["months"] = monthlist;



            var assignedScList = _context.Surveys
                 .Include(s => s.Sc)
                 .Where(s => s.Sc.Id != null)
                 .Select(s => s.Sc.Id)
                 .ToList();



            


            List<string> CoordinatorUserids = _context.UserRoles.Where(a => a.RoleId == CoordinatorRolecode).Select(b => b.UserId).ToList();



                 List<string> unassignedSCs = CoordinatorUserids.Except(assignedScList).ToList();


            List<ApplicationUser> coordinators = _userManager.Users.Where(a => unassignedSCs.Any(c => c == a.Id)).ToList();

            ViewData["Coordinators"] = new SelectList((from s in coordinators
                                                    select new { Scid = s.Id, FullName = s.FirstName + " " + s.LastName }).OrderBy(x=>x.FullName), "Scid", "FullName");

            List<string> FocalPersonIds = _context.UserRoles.Where(a => a.RoleId == FocalPersonRolecode).Select(b => b.UserId).ToList();
            List<ApplicationUser> focalpersons = _userManager.Users.Where(a => FocalPersonIds.Any(c => c == a.Id)).ToList();

            
            if (HttpContext.User.IsInRole("ICSC Survey Manager"))
             {
                var test = HttpContext.Session.GetString("_surveyManagerID");
                List<string> currentFocalPersonId = _context.UserRoles
                    .Where(a => a.RoleId == FocalPersonRolecode && a.UserId == test)
                    .Select(b => b.UserId).ToList();
                
                ViewData["FocalPersons"] = new SelectList((from s in _userManager.Users.Where(a => currentFocalPersonId.Any(c => c == a.Id))
                                                           select new { Smid = test, FullName = s.FirstName + " " + s.LastName }), "Smid", "FullName");
            }
            else
            {

                ViewData["FocalPersons"] = new SelectList((from s in focalpersons
                                                           select new { Smid = s.Id, FullName = s.FirstName + " " + s.LastName }), "Smid", "FullName");
            }
            

            return View();



        }

        // POST: Surveys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Dsid,SurveyType,Round,SurveyMonth,SurveyYear,SurveyBegin,SurveyEnd,NoOfStaff,Scid,Smid,Syscode")] Survey survey)
        {
            if (ModelState.IsValid)
            {
                _context.Add(survey);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Dsid"] = new SelectList(_context.DutyStations, "Dsid", "Country", survey.Dsid);
            //ViewData["Scid"] = new SelectList(_context.applicationUsers, "Id", "FirstName", survey.Scid);
            //ViewData["Smid"] = new SelectList(_context.applicationUsers, "Id", "FirstName", survey.Smid);
            ViewData["Scid"] = new SelectList((from s in _context.applicationUsers.ToList()
                                               select new { Scid = s.Id, FullName = s.FirstName + " " + s.LastName }), "Smid", "FullName");
            ViewData["Smid"] = new SelectList((from s in _context.applicationUsers.ToList()
                                               select new { Smid = s.Id, FullName = s.FirstName + " " + s.LastName }), "Smid", "FullName");
            return View(survey);
        }



        //      [Authorize(Roles = "Survey Coordinator, ICSC Focal Personr, Adminstrator")]
        // GET: Surveys/Edit/5
        [Authorize(Roles = "Administrator,ICSC Focal Person, ICSC Survey Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var survey = await _context.Surveys.FindAsync(id);
            if (survey == null)
            {
                return NotFound();
            }

            ViewData["Dsid"] = new SelectList((from s in _context.DutyStations.ToList()
                                               select new { Dsid = s.Dsid, CountryCity = (s.Country + " - " + s.City + " - " + s.Dsid).Trim() }), "Dsid", "CountryCity");
            
            
            

            List<string> CoordinatorUserids = _context.UserRoles.Where(a => a.RoleId == CoordinatorRolecode).Select(b => b.UserId).ToList();
            

            List <ApplicationUser> coordinators = _userManager.Users.Where(a => CoordinatorUserids.Any(c => c == a.Id)).ToList();

            ViewData["Coordinators"] = new SelectList((from s in coordinators
                                                       select new { Scid = s.Id, FullName = s.FirstName + " " + s.LastName }).OrderBy(s=>s.FullName), "Scid", "FullName");

            List<string> FocalPersonIds = _context.UserRoles.Where(a => a.RoleId == FocalPersonRolecode).Select(b => b.UserId).ToList();
            List<ApplicationUser> focalpersons = _userManager.Users.Where(a => FocalPersonIds.Any(c => c == a.Id)).ToList();

            ViewData["FocalPersons"] = new SelectList((from s in focalpersons
                                                       select new { Smid = s.Id, FullName = s.FirstName + " " + s.LastName }), "Smid", "FullName");
            
            
            //ViewData["Scid"] = new SelectList(_context.applicationUsers, "Id", "FirstName", survey.Scid);
            //ViewData["Smid"] = new SelectList(_context.applicationUsers, "Id", "FirstName", survey.Smid);
            
            return View(survey);
        }

        // POST: Surveys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Dsid,SurveyType,Round,SurveyMonth,SurveyYear,SurveyBegin,SurveyEnd,NoOfStaff,Scid,Smid,Syscode")] Survey survey)
        {
            if (id != survey.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(survey);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SurveyExists(survey.Id))
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
            ViewData["Dsid"] = new SelectList(_context.DutyStations, "Dsid", "Country", survey.Dsid);
            ViewData["Scid"] = new SelectList(_context.applicationUsers, "Id", "FistName", survey.Scid);
            ViewData["Smid"] = new SelectList(_context.applicationUsers, "Id", "FirstName", survey.Smid);
            return View(survey);
        }

        // GET: Surveys/Delete/5

        [Authorize(Roles = "Administrator,ICSC Focal Person, ICSC Survey Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var survey = await _context.Surveys
                .Include(s => s.Ds)
                .Include(s => s.Sc)
                .Include(s => s.Sm)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        // POST: Surveys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var survey = await _context.Surveys.FindAsync(id);
            _context.Surveys.Remove(survey);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public string GetRoleID(string roleName)
        {
            string x = _context.Roles.Where(ab => ab.Name == roleName).Select(bb => bb.Id).Distinct().FirstOrDefault().ToString();
            return (x);
        }
        public IList UsersWithRole(string rolename)
        {
            var userThatYouWant = _userManager.GetUsersInRoleAsync("Administrator");
            //   var s = await _userManager.GetUsersInRoleAsync(rolename);

            return ((IList)userThatYouWant);
        }

        private bool SurveyExists(int id)
        {
            return _context.Surveys.Any(e => e.Id == id);
        }

        
        private static List<SurveyTypeName> GetSurveyType()
        {
            return new List<SurveyTypeName>()
            {
                new SurveyTypeName(){shortName = "PP", longName="Place to Place"},
                new SurveyTypeName(){shortName = "H", longName="Housing"}
            };
        }

        
    }
}
