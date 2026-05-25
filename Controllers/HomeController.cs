using CSMS.Data;
using CSMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.Web;
using System.Net.Http;

namespace CSMS.Controllers
{
    public class HomeController : Controller
    {


        private readonly UserManager<ApplicationUser> _userManager;
   
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private string myTry;

        private DateTime todaysDate = DateTime.UtcNow;
        private DateTime pastSurveyCutOffDate = DateTime.UtcNow.AddMonths(-6);
        private DateTime pastSurveyCutOffDate2016 = DateTime.UtcNow.AddMonths(-1);
        public HomeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context/*, IDMSDbContext idmsdbcontext*/)
        {
            //_logger = logger;
            _context = context;
            //    _IDMScontext = idmsdbcontext;
            _userManager = userManager;
            // _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public  IActionResult ICSCDashboard()
        {
            return View();
        }

        public IActionResult DisplayDashboard(string userID)
        {
            return View();
        }

        public IActionResult OutOfArea()
        {
            HttpContext.Session.SetString("_outOfArea", "1");
            return View();
        }


        public IActionResult Index(int id)
        {
            HttpContext.Session.SetString("_outOfArea", "0");
            var dissd = "";
            var currentUserId = _userManager.GetUserId(HttpContext.User);
            var userIDD = "";
            var userloggedinchecker = HttpContext.Session.GetString("_loggedInUserId");

  
            if (!(currentUserId is null))
            {
                userIDD = currentUserId.ToString();
            }

            HttpContext.Session.SetString("_loggedInUserId", userIDD);


            if (_signInManager.IsSignedIn(User))
            {

                if (HttpContext.User.IsInRole("Survey Coordinator"))
                {
                    HttpContext.Session.SetString("_userRole", "Coordinator");


                    var activeSurvey = _context.Surveys
                        .Include(a => a.Ds)
                        .Where(a => a.Scid == currentUserId.ToString()
                          && a.SurveyBegin >= DateTime.Now || a.SurveyEnd >= DateTime.Now);

                    if (activeSurvey is null)
                    {
                        HttpContext.Session.SetString("_coordinatorStatus", "Inactive");
                    }



                    try
                    {

                        dissd = _context.Surveys
                              .Where(a => a.Scid == currentUserId.ToString()).FirstOrDefault().Dsid.ToString().Trim();
                    }
                    catch (Exception e)
                    {
                        myTry = "Error";
                        HttpContext.Session.SetString("_coordinatorStatus", "Error");
                        HttpContext.Session.SetString("_coordinatorError", e.Message);
                        return View("SC_Overview");
                        //throw ;
                    }
                    if (myTry != "Error")
                    {

                        
                        //This will make sure that survey coordinators will only have access to the surveys in the current or future year
                        var thisYear = DateTime.Now.Year.ToString();
                        var nextYear = (DateTime.Now.Year + 1).ToString();
                        var lastYear = (DateTime.Now.Year - 1).ToString();



                        var dsdetails = _context.Surveys.Include(a => a.Ds)
                            .Where(a => a.Dsid == dissd && a.Scid == currentUserId.ToString() && (a.SurveyYear == thisYear || a.SurveyYear == nextYear || a.SurveyYear == lastYear))
                            .Select(a => new { a.NoOfStaff, a.Ds.Country, a.Ds.City, a.SurveyMonth, a.SurveyYear, a.Ds.Dsgroup, a.SurveyType, a.Syscode }).OrderByDescending(b=>b.SurveyYear).FirstOrDefault();

                        HttpContext ctx = HttpContext;

                        var ctry = dsdetails.Country.ToString().Trim();
                        var city = dsdetails.City.ToString().Trim();
                        var dsGroup = dsdetails.Dsgroup.ToString().Trim();
                        var srvMonth = dsdetails.SurveyMonth.ToString().Trim();
                        var srvYear = dsdetails.SurveyYear.ToString().Trim();
                        var srvType = dsdetails.SurveyType.ToString().Trim();
                        var sysCode = dsdetails.Syscode.ToString().Trim();
                        var numStaff = dsdetails.NoOfStaff.ToString().Trim();
                        int month = DateTime.ParseExact(srvMonth, "MMMM", CultureInfo.CurrentCulture).Month;


                        ctx.Session.SetString("_loggedInUserSurveyYear", srvYear);
                        ctx.Session.SetString("_loggedInUserSurveyMonth", srvMonth);
                        ctx.Session.SetString("_loggedInUserCountry", ctry);
                        ctx.Session.SetString("_loggedInUserCity", city);
                        ctx.Session.SetString("_loggedInUserSurvType", srvType);
                        ctx.Session.SetString("_loggedInUserSysCode", sysCode);
                        ctx.Session.SetString("_loggedInUserDSID", dissd);
                        ctx.Session.SetString("_loggedInUserdsGroup", dsGroup);
                        ctx.Session.SetString("_numberOfStaff", numStaff); 


                        ctx.Session.SetString("_loggedInUserMonthNo", month.ToString());
                        ctx.Session.SetString("_Syscode", sysCode);
                        ctx.Session.SetString("_dsGroupToDisplay", dsGroup);
                        ctx.Session.SetString("_dsNameToDisplay", ctry + " - " + city);

                        if (ctx.Session.GetString("_loggedInUserSurvType")!="") 
                        {
                            ctx.Session.SetString("_dsSurveyType", ctx.Session.GetString("_loggedInUserSurvType")); 
                        }
                        else if (ctx.Session.GetString("_dsSurveyTypeToDisplay") != "")
                        {
                            ctx.Session.SetString("_dsSurveyType", ctx.Session.GetString("_dsSurveyTypeToDisplay"));
                        }



                        if (id == 2 | id == 0)
                        {
                            return View("SC_Overview");
                        }
                        else if (id == 1)
                        {
                            return View("Index");
                        }
                        else
                        {
                            HttpContext.Session.Clear();

                            return View("SC_Overview");
                        }

                    }

                }

                else if (HttpContext.User.IsInRole("ICSC Focal Person"))
                {
                    var fpDetails = _userManager.GetUserId(HttpContext.User);
                    HttpContext.Session.SetString("_focalPersonID", fpDetails);
                    HttpContext.Session.SetString("_userRole", "ICSC FP");
                    return View("DisplayDashboard");
                }
                else if (HttpContext.User.IsInRole("Administrator")) 
                {
                    HttpContext.Session.SetString("_userRole", "Adminstrator");
                    return View("DisplayDashboard");
                }
                else if (HttpContext.User.IsInRole("Site Admin"))
                {
                    HttpContext.Session.SetString("_userRole", "SiteAdmin");
                   
                    return View("DisplayDashboard");
                }
                else if (HttpContext.User.IsInRole("ICSC Survey Manager"))
                {
//  ***
                    var smDetails = _userManager.GetUserId(HttpContext.User);
                    HttpContext.Session.SetString("_surveyManagerID", smDetails);
//  ***                   

                    HttpContext.Session.SetString("_userRole", "ICSC SM");
                    return RedirectToAction("DisplayDashboard", "Home");
                }
                else if (HttpContext.User.IsInRole("Pricing Agent"))
                {
                    HttpContext.Session.SetString("_userRole", "PricingAgent");
                    return RedirectToAction("Index", "Document");
 
                }

            }
            else
            {
                if (id == 2)
                {
                    if (HttpContext.Session.GetString("_dsGroupToDisplay") is null)
                    {
                        return View("Index");
                    }
                    return View("SC_Overview");
                }
                else if (id == 1 || id == 0 || id == 3)
                {
                    if (id == 3)
                    {
                        HttpContext.Session.Clear();
                    }
                    if (HttpContext.Session.GetString("_SelectedDsGroup") is null || id == 3)
                    {
                        var DSList = _context.Surveys.Include(a => a.Ds)

                            .Select(aa => new { aa.Dsid, aa.Ds.Country, aa.SurveyBegin, aa.SurveyEnd, aa.Round})
                            .Where(x=>x.SurveyEnd.Value.Year-todaysDate.Year >= -2)
                            .OrderBy(x=>x.SurveyBegin).ThenBy(x => x.Country)
                            .Distinct().ToList();

                        var DSList16 = _context.listOriginals
                            .Select(b => new { b.DutyStationId, b.dsid, b.City, b.SurveyBegin, b.SurveyEnd });
                            
         
                        var query = from x in DSList
                                    select new { Dsid = x.Dsid, Country = x.Country, round = x.Round, SurveyBegin=x.SurveyBegin, SurveyEnd = x.SurveyEnd};

                        var testt = query.Select(x => x.SurveyEnd.Value.Year - todaysDate.Year);

                       
                        var activeListAll = query.Select(x => new { x.Dsid, x.Country, x.SurveyBegin, x.SurveyEnd })
                            .Where(x => x.SurveyBegin <= todaysDate && x.SurveyEnd >= todaysDate).OrderBy(x => x.Country).ToList();
                        var upcomingListAll = query.Select(x => new { x.Dsid, x.Country, x.SurveyBegin, x.SurveyEnd, x.round })
                            .Where(x => x.SurveyBegin > todaysDate).OrderBy(x => x.Country).ToList();
                        var pastListAll = query.Select(x => new { x.Dsid, x.Country, x.SurveyBegin, x.SurveyEnd, x.round })
                            .Where(x => x.SurveyEnd < todaysDate && x.SurveyEnd >= pastSurveyCutOffDate ).OrderBy(x => x.Country).ToList();


                        ViewData["pastListAll"] = new SelectList(pastListAll, "Dsid", "Country");
                        ViewData["ActiveListAll"] = new SelectList(activeListAll, "Dsid", "Country");
                        ViewData["upcomingListAll"] = new SelectList(upcomingListAll, "Dsid", "Country");

                    }
                }

                return View();

            }


            return View("Index");

        }


        public IActionResult Redirector(int id)

        {

            bool userLoggedIn = _signInManager.IsSignedIn(User);
            string userRole = HttpContext.Session.GetString("_userRole");
            string scAssignedSurveyType = HttpContext.Session.GetString("_loggedInUserSurvType");
            string scAssignedDS = HttpContext.Session.GetString("_loggedInUserSurvType");
            string scActiveStatus = HttpContext.Session.GetString("_coordinatorStatus");
            string scDSgroup = HttpContext.Session.GetString("_loggedInUserdsGroup");


            string selectedDS = HttpContext.Session.GetString("_selectedDSCountry");

            if (userRole == "Coordinator")
            {
                //if (scActiveStatus == null){

                if (scAssignedSurveyType == "Housing")
                {
                    return (View("SC_HousingOverview"));
                }
                else
                {
                    if (scDSgroup == "1")
                    {
                        return (View("SC_Overview1"));
                    }
                    else if (scDSgroup == "2")
                    {
                        return (View("SC_Overview2"));
                    }

                }
                //}else{}


            }
            else if (userRole == "ICSC Focal Person" | userRole == "Administrator")
            {

            }




            return (View());
        }


        public IActionResult SelectDS()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SelectDS(string dsIdSelected)
        {


            var dsInfo = _context.Surveys.Include(a => a.Ds)
                                                .Select(aa => new { aa.Dsid, aa.Ds.Country, aa.Ds.City, aa.Ds.Dsgroup, aa.SurveyBegin, aa.SurveyEnd, aa.SurveyType, aa.Round }).FirstOrDefault();
            var dsInfo2016 = _context.listOriginals.Select(bb => new { bb.dsid, bb.City, bb.survyear, bb.survdate, bb.DutyStationId, bb.Roundno, bb.survtype, bb.SurveyBegin, bb.SurveyEnd }).FirstOrDefault();
            if (dsIdSelected != null)
            {
                char survTiming = dsIdSelected[dsIdSelected.Length - 1];
                dsIdSelected= dsIdSelected.Substring(0, dsIdSelected.Length - 1).Trim();

                

                if (survTiming == '0')//Active Surveys
                {
                    dsInfo = _context.Surveys.Include(a => a.Ds)
                        .Select(aa => new { aa.Dsid, aa.Ds.Country, aa.Ds.City, aa.Ds.Dsgroup, aa.SurveyBegin, aa.SurveyEnd, aa.SurveyType, aa.Round })
                        .Where(x => x.Dsid == dsIdSelected && x.SurveyBegin <= todaysDate && x.SurveyEnd > todaysDate).FirstOrDefault();
                    if (dsInfo == null) 
                    {
                        dsInfo2016 = _context.listOriginals
                                 .Select(bb => new { bb.dsid, bb.City, bb.survyear, bb.survdate, bb.DutyStationId, bb.Roundno, bb.survtype, bb.SurveyBegin, bb.SurveyEnd })
                                 .Where(b => b.SurveyBegin <= todaysDate && b.SurveyEnd > todaysDate && b.DutyStationId == dsIdSelected).FirstOrDefault();
                    }
                }
                else if (survTiming == '1')// Upcoming Surveys
                {
                    dsInfo = _context.Surveys.Include(a => a.Ds)
                        .Select(aa => new { aa.Dsid, aa.Ds.Country, aa.Ds.City, aa.Ds.Dsgroup, aa.SurveyBegin, aa.SurveyEnd, aa.SurveyType, aa.Round })
                        .Where(b => b.SurveyBegin > todaysDate && b.Dsid == dsIdSelected).FirstOrDefault();
                    if (dsInfo == null)
                    {
                        dsInfo2016 = _context.listOriginals
                                 .Select(bb => new { bb.dsid, bb.City, bb.survyear, bb.survdate, bb.DutyStationId, bb.Roundno, bb.survtype, bb.SurveyBegin, bb.SurveyEnd })
                                 .Where(b => b.SurveyBegin > todaysDate && b.DutyStationId == dsIdSelected).FirstOrDefault();
                    }
                }
                else //Past Surveys
                {
                    dsInfo = _context.Surveys.Include(a => a.Ds)
                         .Select(aa => new { aa.Dsid, aa.Ds.Country, aa.Ds.City, aa.Ds.Dsgroup, aa.SurveyBegin, aa.SurveyEnd, aa.SurveyType, aa.Round })
                         .Where(b => b.SurveyEnd >= pastSurveyCutOffDate && b.SurveyEnd < todaysDate && b.Dsid == dsIdSelected).FirstOrDefault();
                    if (dsInfo == null)
                    {
                        dsInfo2016 = _context.listOriginals
                                 .Select(bb => new { bb.dsid, bb.City, bb.survyear, bb.survdate, bb.DutyStationId, bb.Roundno, bb.survtype, bb.SurveyBegin, bb.SurveyEnd })
                                 .Where(b => b.SurveyEnd >= pastSurveyCutOffDate && b.SurveyEnd < todaysDate && b.DutyStationId == dsIdSelected).FirstOrDefault();
                    }
                    
                    
                }
                
                if (dsInfo == null)
                { 
                    //string redirectUrlInfo = "TUR001-ANKARA#H*122023";
                    string redirectUrlInfo =dsInfo2016.dsid + "-" + dsInfo2016.City + dsInfo2016.Roundno + "#" + dsInfo2016.survtype + "*" + MonthNumber(dsInfo2016.survdate) + dsInfo2016.survyear;
                       
                   return RedirectTo2016(redirectUrlInfo);
                }
                else
                {

                    HttpContext.Session.SetString("_SelectedDsGroup", dsInfo.Dsgroup.ToString().Trim());
                    HttpContext.Session.SetString("_SelectedDsCountry", dsInfo.Country.ToString().Trim());
                    HttpContext.Session.SetString("_SelectedDsCity", dsInfo.City.ToString().Trim());
                    HttpContext.Session.SetString("_SelectedDsSurveyType", dsInfo.SurveyType.ToString().Trim());
                    HttpContext.Session.SetString("_SelectedDsDSID", dsIdSelected.ToString().Trim());
                    HttpContext.Session.SetString("_dsGroupToDisplay", dsInfo.Dsgroup.ToString().Trim());
                    HttpContext.Session.SetString("_dsSurveyTypeToDisplay", dsInfo.SurveyType.ToString().Trim());
                    HttpContext.Session.SetString("_dsSurveyRoundToDisplay", dsInfo.Round.ToString().Trim());
                    

                    HttpContext.Session.SetString("_dsNameToDisplay", dsInfo.Country.ToString().Trim() + " - " + dsInfo.City.ToString().Trim());

                    return View("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        public IActionResult Questionnaires()
        {
            return View("Questionnaires");
        }


        // This Method determines if the current logged survey coordinator completed a survey or not

        public IActionResult RedirectTo2016(string urltoredirect)
        
        {
            string submitted = "Go";
            string encodedDsid = HttpUtility.UrlEncode(urltoredirect);
            string redirectUrl = $"https://unicsc.org/COLSurveys/redirector.asp?dsid={encodedDsid}&Submitted={submitted}";
            return Redirect(redirectUrl);

        }


        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult MonitoringTool()
        {
            return View();
        }

        public IActionResult Vertical()
        {
            return View();
        }

        public IActionResult getScMonitoringReport(string dsid)
        {
            var dsidd = dsid.Replace("'", "");


            SqlParameter dsidParameter = new SqlParameter("@ds", dsid);

            if (dsid == "'ITA001'" || dsid == "'POL005'" || dsid == "'CAN004'" || dsid == "'CAN001'" || dsid == "'CAN006'" || dsid == "'FRA001'" || dsid == "'FRA002'")
            {
                dsid = dsid.Substring(0, 4) + "'";
                var test = _context.scMontoringReport
                // .FromSqlRaw("execute ICC_COLD_SURVEYS_2021.dbo.cold_web_report1 " + dsid)
                .FromSqlRaw("execute ICC_COLD_SURVEYS_2021.dbo.cold_web_report2 " + dsid)
                .ToList();

                return Ok(test);
            }

            else
            {

                var test1 = _context.scMontoringReport

                .FromSqlRaw("execute ICC_COLD_SURVEYS_2021.dbo.cold_web_report1 " + dsid)
                .ToList();

                return Ok(test1);
            }

        }
        public int MonthNumber(string monthName)
        {

            DateTime month = DateTime.ParseExact(monthName, "MMMM", CultureInfo.CurrentCulture);
            return month.Month;
        }



       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
