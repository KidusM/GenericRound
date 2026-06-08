using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CSMS.Models;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
using System.IO.Compression;

namespace CSMS.Controllers
{
    public class DocumentUploadController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public DocumentUploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        private string GetOrCreateFolderCaseInsensitive(string parentPath, string folderName)
        {
            Directory.CreateDirectory(parentPath);

            var existingFolder = Directory.GetDirectories(parentPath)
                .FirstOrDefault(d =>
                    string.Equals(
                        Path.GetFileName(d),
                        folderName,
                        StringComparison.OrdinalIgnoreCase));

            if (existingFolder != null)
            {
                return existingFolder;
            }

            var newFolderPath = Path.Combine(parentPath, folderName);
            Directory.CreateDirectory(newFolderPath);

            return newFolderPath;
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

        private string GetDocumentsRootPath()
        {
            var assetsPath = GetRealFolderName(_environment.WebRootPath, "Assets");
            return GetRealFolderName(assetsPath, "Documents");
        }

        private string GetLatestDynamicPartialRoundFolder()
        {
            var dynamicPartialsPath = Path.Combine(
                _environment.WebRootPath,
                "DynamicPartials");

            if (!Directory.Exists(dynamicPartialsPath))
            {
                return null;
            }

            var latestYearFolder = Directory.GetDirectories(dynamicPartialsPath)
                .Select(d => Path.GetFileName(d))
                .Where(name => int.TryParse(name, out _))
                .OrderByDescending(name => int.Parse(name))
                .FirstOrDefault();

            if (latestYearFolder == null)
            {
                return null;
            }

            return Path.Combine(dynamicPartialsPath, latestYearFolder);
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!IsSiteAdmin())
            {
                return View("~/Views/Shared/Unauthorized.cshtml");
            }

            var model = new DocumentUpload
            {
                UploadedFiles = GetUploadedFiles()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(DocumentUpload model)
        {
            if (!IsSiteAdmin())
            {
                return View("~/Views/Shared/Unauthorized.cshtml");
            }

            if (model.Files == null || model.Files.Count == 0)
            {
                ModelState.AddModelError("Files", "Please select at least one file.");
                return View(model);
            }

            var allowedExtensions = new[] { ".xlsx", ".xls", ".pdf", ".doc", ".docx", ".xlsm" };

            foreach (var file in model.Files)
            {
                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Files", $"File type not allowed: {originalFileName}");
                    return View(model);
                }

                int closingBracketIndex = originalFileName.IndexOf("]");

                if (!originalFileName.StartsWith("[") || closingBracketIndex == -1)
                {
                    ModelState.AddModelError("Files", $"Invalid file name format: {originalFileName}");
                    return View(model);
                }

                var folderStructure = originalFileName.Substring(1, closingBracketIndex - 1);
                var folderParts = folderStructure.Split('-');

                var newFileName = originalFileName.Substring(closingBracketIndex + 1).Trim();

                var rootPath = GetDocumentsRootPath();
                var folderPath = rootPath;

                foreach (var part in folderParts)
                {
                    folderPath = GetOrCreateFolderCaseInsensitive(folderPath, part);
                }

                var filePath = Path.Combine(folderPath, newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            ViewBag.Message = "Files uploaded successfully.";
            model.UploadedFiles = GetUploadedFiles();

            return View(model);
        }

        [HttpGet]
        public IActionResult DownloadDynamicPartialsRound()
        {
            if (!IsSiteAdmin())
            {
                return View("~/Views/Shared/Unauthorized.cshtml");
            }

            var roundPath = GetLatestDynamicPartialRoundFolder();

            if (roundPath == null)
            {
                TempData["RoundError"] = "No round folder was found in DynamicPartials.";
                return RedirectToAction(nameof(Index));
            }

            var year = Path.GetFileName(roundPath);

            var files = Directory.GetFiles(roundPath, "*.*", SearchOption.AllDirectories);

            if (files.Length == 0)
            {
                TempData["RoundError"] = $"The DynamicPartials folder for {year} has no files to download.";
                return RedirectToAction(nameof(Index));
            }

            var memoryStream = new MemoryStream();

            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var file in files)
                {
                    var relativePath = Path.GetRelativePath(roundPath, file);
                    archive.CreateEntryFromFile(file, relativePath);
                }
            }

            memoryStream.Position = 0;

            return File(
                memoryStream,
                "application/zip",
                $"DynamicPartials-{year}.zip");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadDynamicPartialsRound(IFormFile dynamicPartialsZipFile)
        {
            if (!IsSiteAdmin())
            {
                return View("~/Views/Shared/Unauthorized.cshtml");
            }

            if (dynamicPartialsZipFile == null)
            {
                TempData["RoundError"] = "Please select a zip file.";
                return RedirectToAction(nameof(Index));
            }

            var extension = Path.GetExtension(dynamicPartialsZipFile.FileName).ToLower();

            if (extension != ".zip")
            {
                TempData["RoundError"] = "Only .zip files are allowed.";
                return RedirectToAction(nameof(Index));
            }

            var roundPath = GetLatestDynamicPartialRoundFolder();

            if (roundPath == null)
            {
                TempData["RoundError"] = "No round folder was found in DynamicPartials.";
                return RedirectToAction(nameof(Index));
            }

            var safeRoundPath = Path.GetFullPath(roundPath);

            using (var stream = dynamicPartialsZipFile.OpenReadStream())
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                foreach (var entry in archive.Entries)
                {
                    if (string.IsNullOrWhiteSpace(entry.Name))
                    {
                        continue;
                    }

                    var destinationPath = Path.GetFullPath(
                        Path.Combine(roundPath, entry.FullName));

                    if (!destinationPath.StartsWith(safeRoundPath))
                    {
                        return BadRequest("Invalid zip file path.");
                    }

                    Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

                    entry.ExtractToFile(destinationPath, true);
                }
            }

            TempData["RoundMessage"] = "DynamicPartials round files uploaded successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(string currentPath)
        {
            if (!IsSiteAdmin())
            {
                return View("~/Views/Shared/Unauthorized.cshtml");
            }

            var model = GetFolderBrowserModel(currentPath);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadToCurrentFolder(string currentPath, List<IFormFile> uploadFiles)
        {
            if (!IsSiteAdmin())
            {
                return View("~/Views/Shared/Unauthorized.cshtml");
            }

            currentPath = currentPath ?? "";

            if (uploadFiles == null || uploadFiles.Count == 0)
            {
                TempData["UploadError"] = "Please select a file.";
                return RedirectToAction(nameof(Delete), new { currentPath });
            }

            var file = uploadFiles.FirstOrDefault();

            if (file == null)
            {
                TempData["UploadError"] = "Please select a file.";
                return RedirectToAction(nameof(Delete), new { currentPath });
            }

            var allowedExtensions = new[] { ".xlsx", ".xls", ".pdf", ".doc", ".docx" };

            var originalFileName = Path.GetFileName(file.FileName);
            var extension = Path.GetExtension(originalFileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                TempData["UploadError"] = "File type not allowed.";
                return RedirectToAction(nameof(Delete), new { currentPath });
            }

            var rootPath = GetDocumentsRootPath();
            Directory.CreateDirectory(rootPath);

            var safeRootPath = Path.GetFullPath(rootPath);
            var folderPath = Path.GetFullPath(Path.Combine(rootPath, currentPath));

            if (!folderPath.StartsWith(safeRootPath))
            {
                return BadRequest("Invalid folder path.");
            }

            if (!Directory.Exists(folderPath))
            {
                TempData["UploadError"] = "Selected folder does not exist.";
                return RedirectToAction(nameof(Delete), new { currentPath = "" });
            }

            var fullFilePath = Path.GetFullPath(Path.Combine(folderPath, originalFileName));

            if (!fullFilePath.StartsWith(safeRootPath))
            {
                return BadRequest("Invalid file path.");
            }

            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            TempData["UploadMessage"] = "File uploaded successfully.";

            return RedirectToAction(nameof(Delete), new { currentPath });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFile(string currentPath, string fileName)
        {
            if (!IsSiteAdmin())
            {
                return View("~/Views/Shared/Unauthorized.cshtml");
            }

            currentPath = currentPath ?? "";

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return RedirectToAction(nameof(Delete), new { currentPath });
            }

            var rootPath = GetDocumentsRootPath();
            var safeRootPath = Path.GetFullPath(rootPath);

            var fullPath = Path.GetFullPath(Path.Combine(rootPath, currentPath, fileName));

            if (!fullPath.StartsWith(safeRootPath))
            {
                return BadRequest("Invalid file path.");
            }

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            return RedirectToAction(nameof(Delete), new { currentPath });
        }

        private List<string> GetUploadedFiles()
        {
            var rootPath = GetDocumentsRootPath();

            if (!Directory.Exists(rootPath))
            {
                return new List<string>();
            }

            return Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories)
                .Select(file => Path.GetRelativePath(rootPath, file))
                .ToList();
        }

        private DocumentUpload GetFolderBrowserModel(string currentPath)
        {
            var rootPath = GetDocumentsRootPath();
            Directory.CreateDirectory(rootPath);

            currentPath = currentPath ?? "";

            var safeRootPath = Path.GetFullPath(rootPath);
            var fullCurrentPath = Path.GetFullPath(Path.Combine(rootPath, currentPath));

            if (!fullCurrentPath.StartsWith(safeRootPath))
            {
                currentPath = "";
                fullCurrentPath = safeRootPath;
            }

            return new DocumentUpload
            {
                CurrentPath = currentPath,

                Folders = Directory.GetDirectories(fullCurrentPath)
                    .Select(Path.GetFileName)
                    .ToList(),

                UploadedFiles = Directory.GetFiles(fullCurrentPath)
                    .Select(Path.GetFileName)
                    .ToList()
            };
        }

        private bool IsSiteAdmin()
        {
            return HttpContext.Session.GetString("_userRole") == "SiteAdmin";
        }
    }
}