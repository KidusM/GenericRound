using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMS.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace CSMS.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IWebHostEnvironment _iweb;

        public DocumentController(IWebHostEnvironment iweb)
        {
            _iweb = iweb;
        }
        [Route("Documents")]
        public IActionResult Index(int id)
        {
            string actualPath = "";

            string sRole = HttpContext.Session.GetString("_userRole");

            // determine duty station group for the user
            string dgroup = HttpContext.Session.GetString("_dsGroupToDisplay");
            string surveyType = HttpContext.Session.GetString("_dsSurveyTypeToDisplay");
            string surveyRound = HttpContext.Session.GetString("_dsSurveyRoundToDisplay");

            string separator = System.IO.Path.DirectorySeparatorChar.ToString();

            if (surveyType == null)
            { surveyType = HttpContext.Session.GetString("_loggedInUserSurvType"); }

            if (dgroup != null)
            {
                string pathGroup = "";
                string pathSurveyType = "";
                if (surveyType == "PP") { pathSurveyType = "PP"; } else { pathSurveyType = "HH"; }
                if (dgroup == "0") { pathGroup = "HQ"; }
                else if (dgroup == "1") { pathGroup = "GI"; }
                else if (dgroup == "2") { pathGroup = "GII"; }

                string actualPathPartial = "Assets" + separator + "Documents" + separator + surveyRound + separator + pathGroup + separator + pathSurveyType;

                if (id != 0)
                {
                    if (id == 2 || sRole == "Coordinator") //survey coordinator
                    {
                        //if (surveyType == "H")
                        //{
                        //    actualPath = "Assets"  + separator + "Documents" + separator + surveyRound + separator + "Housing" + separator + "Coordinator";
                        //}
                        //else
                        //{
                        //    if (dgroup == "1") //Group - I
                        //    {
                        //        actualPath = "Assets" + separator + "Documents" + separator + surveyRound + separator + "Group-I" + separator + "Coordinator";
                        //    }
                        //    else if (dgroup == "2") //Group - II
                        //    {
                        //        actualPath = "Assets" + separator + "Documents" + separator + surveyRound + separator + "Group-II" + separator + "Coordinator";
                        //    }
                        //    else // HQ
                        //    {
                        //        actualPath = "Assets" + separator + "Documents" + separator + surveyRound + separator + "HQ" + separator + "Coordinator";
                        //    }
                        //}
                        actualPath = actualPathPartial + separator + "SC";
                    }
                    else if (id == 1 || sRole == "Staff") //staff
                    {
                        actualPath = actualPathPartial + separator + "ST";
                        
                    }
                    else if (id == 3 || sRole == "Pricing Agent") // Pricing Agent
                    {
                        actualPath = actualPathPartial + separator + "PA";

                        //    if (dgroup == "1")
                        //    {
                        //        actualPath = "Assets" + separator + "Documents" + separator + surveyRound + separator + "Group-I" + separator + "PricingAgent";
                        //    }
                        //    else if (dgroup == "2")
                        //    {
                        //        actualPath = "Assets" + separator + "Documents" + separator + surveyRound + separator + "Group-II" + separator + "PricingAgent";
                        //    }
                        //    else
                        //    {
                        //        actualPath = "Assets" + separator + "Documents" + separator + surveyRound + separator + "HQ" + separator + "PricingAgent";
                        //    }
                        //}

                        //else
                        //{

                        //    //Needs Review
                        //    actualPath = "Assets" + separator + "Documents" + separator + surveyRound + separator + "Group-II" + separator + "Coordinator";
                        //}
                    }
                }
                
            }
            else // ds group not known
            {
                if (id == 9)
                {


                    {
                        actualPath = "Assets" + separator + "Documents" + separator + surveyRound + separator + "OA";
                    }



                }
                else
                {
                    return Redirect("~/");
                   
                }
            }

            ViewData["hqOnly"] = "0";
            ViewData["docPath"] = actualPath;
            string[] displayFileName = new string[50];
            Document dc = new Document();
            var displayDocument = Path.Combine(_iweb.WebRootPath, actualPath);
            DirectoryInfo di = new DirectoryInfo(displayDocument);
            FileInfo[] fileinfo = di.GetFiles();
            for (int i = 0; i <= fileinfo.Length - 1; i++)
            {
                int extent = fileinfo[i].Extension.Length;
                int filenamelength = fileinfo[i].Name.ToString().Length;
                string lastChar = fileinfo[i].Name.Substring(filenamelength - extent - 1, 1);
                if (lastChar == "_")
                {
                    displayFileName[i] = "<span style='color:#AD4500'>* </span>" + fileinfo[i].Name.Substring(3, filenamelength - 3 - extent - 1).ToUpper();
                    ViewData["hqOnly"] = "1";
                }
                else
                {
                    displayFileName[i] = fileinfo[i].Name.Substring(3, filenamelength - 3 - extent).ToUpper();
                }
             

            }
            dc.FileDocument = fileinfo;
            ViewData["FileNameForDisplay"] = displayFileName;
            return View(dc);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile docfile)
        {

            string ext = Path.GetExtension(docfile.FileName);

            if (ext == ".jpg" || ext == ".gif" || ext == ".jpeg" || ext == ".png")
            {
                //var docSave = Path.Combine(_iweb.WebRootPath + "\\lib\\Assets\\images", docfile.FileName;
                var docSave = Path.Combine(_iweb.WebRootPath, "lib" + Path.DirectorySeparatorChar + "Assets" + Path.DirectorySeparatorChar + "images", docfile.FileName);
                var stream = new FileStream(docSave, FileMode.Create);
                await docfile.CopyToAsync(stream);
                stream.Close();

            }
            return RedirectToAction("Index");
        }

        public string DisplayTitle(string docName)
        {
            return (docName.Substring(3));
        }
    }
}
