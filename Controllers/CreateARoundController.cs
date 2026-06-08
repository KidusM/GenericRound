using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System;

namespace CSMS.Controllers
{
    public class CreateARoundController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public CreateARoundController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        private string GetRealFolderName(string parentPath, string folderName)
        {
            if (!Directory.Exists(parentPath))
            {
                return Path.Combine(parentPath, folderName);
            }

            var match = Directory.GetDirectories(parentPath)
                .FirstOrDefault(d =>
                    string.Equals(
                        Path.GetFileName(d),
                        folderName,
                        StringComparison.OrdinalIgnoreCase));

            return match ?? Path.Combine(parentPath, folderName);
        }

        private string GetDocumentsRoot()
        {
            var assetsPath = GetRealFolderName(_environment.WebRootPath, "Assets");
            return GetRealFolderName(assetsPath, "Documents");
        }

        private void LoadAvailableRounds()
        {
            string documentsRoot = GetDocumentsRoot();

            if (!Directory.Exists(documentsRoot))
            {
                ViewBag.AvailableRounds = new string[0];
                return;
            }

            ViewBag.AvailableRounds = Directory.GetDirectories(documentsRoot)
                .Select(Path.GetFileName)
                .OrderByDescending(x => x)
                .ToList();
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!IsSiteAdmin())
            {
                return View("~/Views/Shared/Unauthorized.cshtml");
            }

            LoadAvailableRounds();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string year)
        {
            if (!IsSiteAdmin())
            {
                return View("~/Views/Shared/Unauthorized.cshtml");
            }

            if (string.IsNullOrWhiteSpace(year))
            {
                ViewBag.Message = "Please enter a year.";
                LoadAvailableRounds();
                return View();
            }

            year = year.Trim();

            string documentsRoot = GetDocumentsRoot();
            string templateYear = "2026";
            string sourcePath = GetRealFolderName(documentsRoot, templateYear);
            string newYearPath = Path.Combine(documentsRoot, year);

            if (!Directory.Exists(sourcePath))
            {
                ViewBag.Message = $"Template year folder {templateYear} not found.";
                LoadAvailableRounds();
                return View();
            }

            if (Directory.Exists(newYearPath))
            {
                ViewBag.Message = $"Folder {year} already exists.";
                LoadAvailableRounds();
                return View();
            }

            Directory.CreateDirectory(newYearPath);
            CopyFoldersOnly(sourcePath, newYearPath);

            CreateWelcomePagesForNewRound(year);

            ViewBag.Message = $"Round {year} was created successfully.";
            ViewBag.RoundCreated = true;
            ViewBag.CreatedRound = year;

            LoadAvailableRounds();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRound(string yearToDelete)
        {
            if (!IsSiteAdmin())
            {
                return View("~/Views/Shared/Unauthorized.cshtml");
            }

            if (string.IsNullOrWhiteSpace(yearToDelete))
            {
                ViewBag.Message = "Please enter a year to delete.";
                LoadAvailableRounds();
                return View("Index");
            }

            yearToDelete = yearToDelete.Trim();

            string documentsRoot = GetDocumentsRoot();
            string roundPath = GetRealFolderName(documentsRoot, yearToDelete);

            if (!Directory.Exists(roundPath))
            {
                ViewBag.Message = $"Folder {yearToDelete} does not exist.";
                LoadAvailableRounds();
                return View("Index");
            }

            bool hasFiles = Directory.GetFiles(
                roundPath,
                "*",
                SearchOption.AllDirectories
            ).Any();

            if (hasFiles)
            {
                ViewBag.Message =
                    $"Cannot delete {yearToDelete}. One or more folders contain documents.";

                LoadAvailableRounds();
                return View("Index");
            }

            Directory.Delete(roundPath, recursive: true);
            DeleteWelcomePagesForRound(yearToDelete);

            ViewBag.Message = $"Round {yearToDelete} was deleted successfully.";

            ViewBag.RoundDeleted = true;
            ViewBag.DeletedRound = yearToDelete;

            LoadAvailableRounds();
            return View("Index");
        }

        private void CopyFoldersOnly(string sourceDir, string destinationDir)
        {
            foreach (
                string directory in Directory.GetDirectories(
                    sourceDir,
                    "*",
                    SearchOption.AllDirectories
                )
            )
            {
                string relativePath = Path.GetRelativePath(sourceDir, directory);
                string newDirectory = Path.Combine(destinationDir, relativePath);

                Directory.CreateDirectory(newDirectory);
            }
        }

        private void CreateWelcomePagesForNewRound(string year)
        {
            string dynamicPartialsRoot = Path.Combine(_environment.WebRootPath, "DynamicPartials");

            if (!Directory.Exists(dynamicPartialsRoot))
            {
                Directory.CreateDirectory(dynamicPartialsRoot);
            }

            string newRoundFolder = Path.Combine(dynamicPartialsRoot, year);

            if (!Directory.Exists(newRoundFolder))
            {
                Directory.CreateDirectory(newRoundFolder);
            }

            foreach (string file in Directory.GetFiles(dynamicPartialsRoot))
            {
                string fileName = Path.GetFileName(file);
                string destinationFile = Path.Combine(newRoundFolder, fileName);

                System.IO.File.Copy(file, destinationFile, overwrite: false);
            }
        }

        private void DeleteWelcomePagesForRound(string year)
        {
            string dynamicPartialsRoot =
                Path.Combine(_environment.WebRootPath, "DynamicPartials");

            string roundWelcomePagesFolder =
                Path.Combine(dynamicPartialsRoot, year);

            if (Directory.Exists(roundWelcomePagesFolder))
            {
                Directory.Delete(roundWelcomePagesFolder, recursive: true);
            }
        }

        private bool IsSiteAdmin()
        {
            return HttpContext.Session.GetString("_userRole") == "SiteAdmin";
        }
    }
}