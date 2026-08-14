using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using CSMS.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;

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
            string sRole = HttpContext.Session.GetString("_userRole");

            string dgroup = HttpContext.Session.GetString("_dsGroupToDisplay");
            string surveyType = HttpContext.Session.GetString("_dsSurveyTypeToDisplay");
            string surveyRound = HttpContext.Session.GetString("_dsSurveyRoundToDisplay");

            if (string.IsNullOrWhiteSpace(surveyType))
            {
                surveyType = HttpContext.Session.GetString("_loggedInUserSurvType");
            }

            // Survey round is required to locate documents
            if (string.IsNullOrWhiteSpace(surveyRound))
            {
                return Redirect("~/");
            }

            string pathGroup = "";
            string pathSurveyType = "";

            if (dgroup != null)
            {
                pathSurveyType = string.Equals(
                    surveyType,
                    "PP",
                    StringComparison.OrdinalIgnoreCase)
                    ? "PP"
                    : "HH";

                switch (dgroup)
                {
                    case "0":
                        pathGroup = "HQ";
                        break;

                    case "1":
                        pathGroup = "GI";
                        break;

                    case "2":
                        pathGroup = "GII";
                        break;

                    default:
                        return Redirect("~/");
                }
            }

            /*
             * Build the requested path as individual folder names.
             *
             * ResolveDirectoryPath() below will locate the folders
             * case-insensitively. This is important on Linux because:
             *
             * Assets != assets
             * Documents != documents
             */
            List<string> folders = new List<string>();

            if (dgroup != null)
            {
                folders.Add("Assets");
                folders.Add("Documents");
                folders.Add(surveyRound);
                folders.Add(pathGroup);
                folders.Add(pathSurveyType);

                switch (id)
                {
                    case 1:
                        folders.Add("ST");
                        break;

                    case 2:
                        folders.Add("SC");
                        break;

                    case 3:
                        folders.Add("PA");
                        break;

                    default:
                        return Redirect("~/");
                }
            }
            else
            {
                if (id == 9)
                {
                    folders.Add("Assets");
                    folders.Add("Documents");
                    folders.Add(surveyRound);
                    folders.Add("OA");
                }
                else
                {
                    return Redirect("~/");
                }
            }

            // Find actual directory using case-insensitive folder matching.
            string displayDocument = ResolveDirectoryPath(
                _iweb.WebRootPath,
                folders.ToArray()
            );

            /*
             * If the directory genuinely does not exist, return an empty
             * document list instead of crashing the application.
             */
            if (string.IsNullOrWhiteSpace(displayDocument) ||
                !Directory.Exists(displayDocument))
            {
                Document emptyDocument = new Document();

                emptyDocument.FileDocument = Array.Empty<FileInfo>();

                ViewData["hqOnly"] = "0";
                ViewData["docPath"] = "";
                ViewData["FileNameForDisplay"] = Array.Empty<string>();

                return View(emptyDocument);
            }

            /*
             * Create a relative path for the View.
             * This uses the REAL capitalization found on the server.
             */
            string actualPath = Path.GetRelativePath(
                _iweb.WebRootPath,
                displayDocument
            );

            ViewData["hqOnly"] = "0";
            ViewData["docPath"] = actualPath;

            DirectoryInfo di = new DirectoryInfo(displayDocument);

            FileInfo[] fileinfo = di
                .GetFiles()
                .OrderBy(f => f.Name)
                .ToArray();

            // Do not limit this to 50 files.
            string[] displayFileName = new string[fileinfo.Length];

            for (int i = 0; i < fileinfo.Length; i++)
            {
                string fileName = fileinfo[i].Name;
                string extension = fileinfo[i].Extension;

                int extent = extension.Length;
                int fileNameLength = fileName.Length;

                /*
                 * Your original naming convention assumes the first
                 * three characters should not be displayed.
                 *
                 * Keep that behavior, but protect against short filenames.
                 */
                if (fileNameLength > 3 + extent)
                {
                    int characterPosition = fileNameLength - extent - 1;

                    string lastChar = "";

                    if (characterPosition >= 0 &&
                        characterPosition < fileNameLength)
                    {
                        lastChar = fileName.Substring(characterPosition, 1);
                    }

                    if (lastChar == "_")
                    {
                        int displayLength = fileNameLength - 3 - extent - 1;

                        if (displayLength > 0)
                        {
                            displayFileName[i] =
                                "<span style='color:#AD4500'>* </span>" +
                                fileName.Substring(3, displayLength).ToUpper();
                        }
                        else
                        {
                            displayFileName[i] = fileName.ToUpper();
                        }

                        ViewData["hqOnly"] = "1";
                    }
                    else
                    {
                        int displayLength = fileNameLength - 3 - extent;

                        if (displayLength > 0)
                        {
                            displayFileName[i] =
                                fileName.Substring(3, displayLength).ToUpper();
                        }
                        else
                        {
                            displayFileName[i] = fileName.ToUpper();
                        }
                    }
                }
                else
                {
                    // Do not crash if somebody uploads an unusually short filename.
                    displayFileName[i] =
                        Path.GetFileNameWithoutExtension(fileName).ToUpper();
                }
            }

            Document dc = new Document();

            dc.FileDocument = fileinfo;

            ViewData["FileNameForDisplay"] = displayFileName;

            return View(dc);
        }


        [HttpPost]
        public async Task<IActionResult> Index(IFormFile docfile)
        {
            if (docfile == null || string.IsNullOrWhiteSpace(docfile.FileName))
            {
                return RedirectToAction("Index");
            }

            string ext = Path
                .GetExtension(docfile.FileName)
                .ToLowerInvariant();

            string[] allowedExtensions =
            {
                ".jpg",
                ".gif",
                ".jpeg",
                ".png"
            };

            if (allowedExtensions.Contains(ext))
            {
                /*
                 * Locate:
                 *
                 * wwwroot/lib/Assets/images
                 *
                 * without depending on capitalization.
                 */
                string imageFolder = ResolveDirectoryPath(
                    _iweb.WebRootPath,
                    "lib",
                    "Assets",
                    "images"
                );

                /*
                 * If this is an upload folder and doesn't already exist,
                 * create it using the expected folder structure.
                 */
                if (string.IsNullOrWhiteSpace(imageFolder))
                {
                    imageFolder = Path.Combine(
                        _iweb.WebRootPath,
                        "lib",
                        "Assets",
                        "images"
                    );

                    Directory.CreateDirectory(imageFolder);
                }

                /*
                 * Path.GetFileName prevents a supplied filename from
                 * injecting another path.
                 */
                string safeFileName = Path.GetFileName(docfile.FileName);

                string docSave = Path.Combine(
                    imageFolder,
                    safeFileName
                );

                using (FileStream stream = new FileStream(
                    docSave,
                    FileMode.Create))
                {
                    await docfile.CopyToAsync(stream);
                }
            }

            return RedirectToAction("Index");
        }


        public string DisplayTitle(string docName)
        {
            if (string.IsNullOrWhiteSpace(docName))
            {
                return "";
            }

            if (docName.Length <= 3)
            {
                return docName;
            }

            return docName.Substring(3);
        }


        /*
         * ============================================================
         * WINDOWS / LINUX SAFE DIRECTORY RESOLVER
         * ============================================================
         *
         * Windows normally treats:
         *
         * Assets
         * assets
         * ASSETS
         *
         * as the same directory.
         *
         * Linux does not.
         *
         * This method walks through each directory and finds the
         * actual directory name ignoring capitalization.
         *
         * Example:
         *
         * Code requests:
         * Assets/Documents/2026/HQ/PP/SC
         *
         * Server actually contains:
         * assets/documents/2026/HQ/PP/SC
         *
         * This method still finds it.
         */
        private string ResolveDirectoryPath(
            string startingDirectory,
            params string[] folders)
        {
            if (string.IsNullOrWhiteSpace(startingDirectory) ||
                !Directory.Exists(startingDirectory))
            {
                return null;
            }

            string currentDirectory = startingDirectory;

            foreach (string folder in folders)
            {
                if (string.IsNullOrWhiteSpace(folder))
                {
                    return null;
                }

                try
                {
                    string exactPath = Path.Combine(
                        currentDirectory,
                        folder
                    );

                    /*
                     * First try exact match because that is fastest.
                     */
                    if (Directory.Exists(exactPath))
                    {
                        currentDirectory = exactPath;
                        continue;
                    }

                    /*
                     * If exact match fails, search the directory
                     * ignoring capitalization.
                     */
                    DirectoryInfo parentDirectory =
                        new DirectoryInfo(currentDirectory);

                    DirectoryInfo matchingDirectory =
                        parentDirectory
                            .GetDirectories()
                            .FirstOrDefault(d =>
                                string.Equals(
                                    d.Name,
                                    folder,
                                    StringComparison.OrdinalIgnoreCase
                                )
                            );

                    if (matchingDirectory == null)
                    {
                        return null;
                    }

                    currentDirectory = matchingDirectory.FullName;
                }
                catch
                {
                    return null;
                }
            }

            return currentDirectory;
        }
    }
}