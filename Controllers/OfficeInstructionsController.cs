using CSMS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

namespace CSMS.Controllers
{
    public class OfficeInstructionsController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public OfficeInstructionsController(IWebHostEnvironment env)
        {
            _env = env;
        }

        private string DataFolder =>
            Path.Combine(_env.ContentRootPath, "App_Data", "OfficeInstructionDivisions");

        private bool IsAdmin()
        {
            string role = HttpContext.Session.GetString("_userRole");

            return role == "SiteAdmin"
                || role == "SuperSiteAdmin";
        }

        private bool IsSuperSiteAdmin()
        {
            return HttpContext.Session.GetString("_userRole")
                   == "SuperSiteAdmin";
        }

        private bool IsSiteAdmin()
        {
            return HttpContext.Session.GetString("_userRole")
                   == "SiteAdmin";
        }



        private string CreateDivisionKey(string divisionName)
        {
            if (string.IsNullOrWhiteSpace(divisionName))
            {
                return "General";
            }

            string key = divisionName.Trim();

            key = Regex.Replace(key, @"[^a-zA-Z0-9]", "");

            if (string.IsNullOrWhiteSpace(key))
            {
                key = "General";
            }

            return key;
        }

        private string GetDivisionFile(string divisionKey)
        {
            return Path.Combine(DataFolder, divisionKey + ".json");
        }

        private OfficeInstructionDivision LoadDivision(string divisionKey)
        {
            if (!Directory.Exists(DataFolder))
            {
                Directory.CreateDirectory(DataFolder);
            }

            string filePath = GetDivisionFile(divisionKey);

            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }

            string json = System.IO.File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<OfficeInstructionDivision>(json);
        }

        private void SaveDivision(OfficeInstructionDivision division)
        {
            if (!Directory.Exists(DataFolder))
            {
                Directory.CreateDirectory(DataFolder);
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string filePath = GetDivisionFile(division.Key);
            string json = JsonSerializer.Serialize(division, options);

            System.IO.File.WriteAllText(filePath, json);
        }

        private List<OfficeInstructionDivision> GetDivisions()
        {
            if (!Directory.Exists(DataFolder))
            {
                Directory.CreateDirectory(DataFolder);
            }

            var divisions = new List<OfficeInstructionDivision>();

            foreach (var file in Directory.GetFiles(DataFolder, "*.json"))
            {
                string json = System.IO.File.ReadAllText(file);

                var division = JsonSerializer.Deserialize<OfficeInstructionDivision>(json);

                if (division != null)
                {
                    divisions.Add(division);
                }
            }

            return divisions
                .OrderBy(x => x.Name)
                .ToList();
        }

        [Route("OfficeInstructions")]
        public IActionResult Divisions()
        {
            ViewBag.IsAdmin = IsAdmin();

            var divisions = GetDivisions();

            return View(divisions);
        }

        [HttpGet]
        [Route("OfficeInstructions/CreateDivision")]
        public IActionResult CreateDivision()
        {
            if (!IsSuperSiteAdmin())
            {
                return Unauthorized();
            }

            return View();
        }

        [HttpPost]
        [Route("OfficeInstructions/CreateDivision")]
        public IActionResult CreateDivision(string divisionName, string passKey)
        {
            if (!IsSuperSiteAdmin())
            {
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(divisionName) || string.IsNullOrWhiteSpace(passKey))
            {
                return RedirectToAction("Divisions");
            }

            string key = CreateDivisionKey(divisionName);

            var existing = LoadDivision(key);

            if (existing == null)
            {
                var division = new OfficeInstructionDivision
                {
                    Key = key,
                    Name = divisionName.Trim(),
                    PassKeyHash = HashPassKey(passKey),
                    Activities = new List<OfficeActivity>()
                };

                SaveDivision(division);
            }

            return RedirectToAction("Index", new { divisionKey = key });
        }


        [HttpGet]
        [Route("OfficeInstructions/{divisionKey}/PassKey")]
        public IActionResult PassKey(string divisionKey)
        {
            var division = LoadDivision(divisionKey);

            if (division == null)
            {
                return NotFound();
            }

            ViewBag.DivisionKey = division.Key;
            ViewBag.DivisionName = division.Name;

            return View();
        }

        [HttpPost]
        [Route("OfficeInstructions/{divisionKey}/PassKey")]
        public IActionResult PassKey(string divisionKey, string passKey)
        {
            var division = LoadDivision(divisionKey);

            if (division == null)
            {
                return NotFound();
            }

            if (division.PassKeyHash == HashPassKey(passKey))
            {
                GrantDivisionAccess(division.Key);

                return RedirectToAction("Index", new
                {
                    divisionKey = division.Key
                });
            }

            ViewBag.DivisionKey = division.Key;
            ViewBag.DivisionName = division.Name;
            ViewBag.Error = "Incorrect passkey.";

            return View();
        }

        [Route("OfficeInstructions/{divisionKey}")]
        public IActionResult Index(string divisionKey)
        {
            var division = LoadDivision(divisionKey);
            if (!HasDivisionAccess(division.Key) && !IsAdmin())
            {
                return RedirectToAction("PassKey", new
                {
                    divisionKey = division.Key
                });
            }

            if (division == null)
            {
                return NotFound();
            }

            ViewBag.IsAdmin = IsAdmin();
            ViewBag.DivisionKey = division.Key;
            ViewBag.DivisionName = division.Name;

            var activities = division.Activities
                .OrderBy(x => x.Title)
                .ToList();

            return View(activities);
        }

        [Route("OfficeInstructions/{divisionKey}/Details/{id}")]
        public IActionResult Details(string divisionKey, int id)
        {
            var division = LoadDivision(divisionKey);

            if (division == null)
            {
                return NotFound();
            }

            var activity = division.Activities
                .FirstOrDefault(x => x.Id == id);

            if (activity == null)
            {
                return NotFound();
            }

            activity.Steps = activity.Steps
                .OrderBy(x => x.StepNumber)
                .ToList();

            ViewBag.IsAdmin = IsAdmin();
            ViewBag.DivisionKey = division.Key;
            ViewBag.DivisionName = division.Name;

            return View(activity);
        }

        [HttpGet]
        [Route("OfficeInstructions/{divisionKey}/Create")]
        public IActionResult Create(string divisionKey)
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }

            var division = LoadDivision(divisionKey);

            if (division == null)
            {
                return NotFound();
            }

            ViewBag.DivisionKey = division.Key;
            ViewBag.DivisionName = division.Name;

            return View(new OfficeActivity());
        }

        [HttpPost]
        [Route("OfficeInstructions/{divisionKey}/Create")]
        public IActionResult Create(string divisionKey, OfficeActivity model)
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }

            var division = LoadDivision(divisionKey);

            if (division == null)
            {
                return NotFound();
            }

            model.Id = division.Activities.Any()
                ? division.Activities.Max(x => x.Id) + 1
                : 1;

            model.Steps = new List<ActivityStep>();

            division.Activities.Add(model);

            SaveDivision(division);

            return RedirectToAction("Edit", new
            {
                divisionKey = division.Key,
                id = model.Id
            });
        }

        [HttpGet]
        [Route("OfficeInstructions/{divisionKey}/Edit/{id}")]
        public IActionResult Edit(string divisionKey, int id)
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }

            var division = LoadDivision(divisionKey);

            if (division == null)
            {
                return NotFound();
            }

            var activity = division.Activities
                .FirstOrDefault(x => x.Id == id);

            if (activity == null)
            {
                return NotFound();
            }

            activity.Steps = activity.Steps
                .OrderBy(x => x.StepNumber)
                .ToList();

            ViewBag.DivisionKey = division.Key;
            ViewBag.DivisionName = division.Name;

            return View(activity);
        }

        [HttpPost]
        [Route("OfficeInstructions/{divisionKey}/Edit/{id}")]
        public IActionResult Edit(string divisionKey, int id, OfficeActivity model)
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }

            var division = LoadDivision(divisionKey);

            if (division == null)
            {
                return NotFound();
            }

            var activity = division.Activities
                .FirstOrDefault(x => x.Id == id);

            if (activity == null)
            {
                return NotFound();
            }

            activity.Title = model.Title;
            activity.ShortDescription = model.ShortDescription;

            if (model.Steps == null)
            {
                model.Steps = new List<ActivityStep>();
            }

            //activity.Steps = model.Steps
            //    .Where(x => !string.IsNullOrWhiteSpace(x.StepTitle)
            //             || !string.IsNullOrWhiteSpace(x.StepDetails))
            //    .Select((x, index) => new ActivityStep
            //    {
            //        StepNumber = index + 1,
            //        StepTitle = x.StepTitle,
            //        StepDetails = x.StepDetails
            //    })
            //    .ToList();

            var newSteps = new List<ActivityStep>();

            var submittedSteps = model.Steps
                .Where(x => !string.IsNullOrWhiteSpace(x.StepTitle)
                         || !string.IsNullOrWhiteSpace(x.StepDetails)
                         || !string.IsNullOrWhiteSpace(x.ScreenshotBase64)
                         || !string.IsNullOrWhiteSpace(x.ScreenshotPath))
                .ToList();

            for (int i = 0; i < submittedSteps.Count; i++)
            {
                var submittedStep = submittedSteps[i];

                int stepNumber = i + 1;

                string screenshotPath = submittedStep.ScreenshotPath;

                if (!string.IsNullOrWhiteSpace(submittedStep.ScreenshotBase64))
                {
                    screenshotPath = SaveScreenshotFromBase64(
                        submittedStep.ScreenshotBase64,
                        division.Key,
                        activity.Id,
                        stepNumber
                    );
                }

                newSteps.Add(new ActivityStep
                {
                    StepNumber = stepNumber,
                    StepTitle = submittedStep.StepTitle,
                    StepDetails = submittedStep.StepDetails,
                    ScreenshotPath = screenshotPath,
                    ScreenshotBase64 = null
                });
            }

            activity.Steps = newSteps;



            SaveDivision(division);

            return RedirectToAction("Details", new
            {
                divisionKey = division.Key,
                id = activity.Id
            });
        }

        [HttpPost]
        [Route("OfficeInstructions/{divisionKey}/Delete/{id}")]
        public IActionResult Delete(string divisionKey, int id)
        {
            if (!IsAdmin())
            {
                return Unauthorized();
            }

            var division = LoadDivision(divisionKey);

            if (division == null)
            {
                return NotFound();
            }

            var activity = division.Activities
                .FirstOrDefault(x => x.Id == id);

            if (activity != null)
            {
                division.Activities.Remove(activity);
                SaveDivision(division);
            }

            return RedirectToAction("Index", new
            {
                divisionKey = division.Key
            });
        }

        private string SaveScreenshotFromBase64(string base64Image, string divisionKey, int activityId, int stepNumber)
        {
            if (string.IsNullOrWhiteSpace(base64Image))
            {
                return null;
            }

            if (!base64Image.StartsWith("data:image"))
            {
                return null;
            }

            string base64Data = base64Image.Substring(base64Image.IndexOf(",") + 1);

            byte[] imageBytes = System.Convert.FromBase64String(base64Data);

            string imageFolder = Path.Combine(
                _env.WebRootPath,
                "OfficeInstructionImages",
                divisionKey
            );

            if (!Directory.Exists(imageFolder))
            {
                Directory.CreateDirectory(imageFolder);
            }

            string fileName = $"activity-{activityId}-step-{stepNumber}.png";

            string fullPath = Path.Combine(imageFolder, fileName);

            System.IO.File.WriteAllBytes(fullPath, imageBytes);

            return $"/OfficeInstructionImages/{divisionKey}/{fileName}";
        }
        private string HashPassKey(string passKey)
        {
            using (var sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(passKey ?? "");
                byte[] hash = sha.ComputeHash(bytes);

                return System.Convert.ToBase64String(hash);
            }
        }

        private bool HasDivisionAccess(string divisionKey)
        {
            return HttpContext.Session.GetString("DivisionAccess_" + divisionKey) == "true";
        }

        private void GrantDivisionAccess(string divisionKey)
        {
            HttpContext.Session.SetString("DivisionAccess_" + divisionKey, "true");
        }

        [HttpPost]
        [Route("OfficeInstructions/DeleteDivision/{divisionKey}")]
        public IActionResult DeleteDivision(string divisionKey)
        {
            if (!IsSuperSiteAdmin())
            {
                return Unauthorized();
            }

            var division = LoadDivision(divisionKey);

            if (division == null)
            {
                return NotFound();
            }

            string filePath = GetDivisionFile(division.Key);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            return RedirectToAction("Divisions");
        }
    }
}