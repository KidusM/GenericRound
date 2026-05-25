using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CSMS.Models;

namespace CSMS.Controllers
{
    public class FAQfromFileController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public FAQfromFileController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult Index()
        {
            var faqFolder = Path.Combine(
                _environment.WebRootPath,
                "documents",
                "FAQ_JSON"
            );

            var faqPages = new List<FaqPageViewModel>();

            if (!Directory.Exists(faqFolder))
            {
                return View(faqPages);
            }

            var jsonFiles = Directory.GetFiles(faqFolder, "*.json");

            foreach (var file in jsonFiles)
            {
                var json = System.IO.File.ReadAllText(file);

                var sections = JsonSerializer.Deserialize<List<FaqSection>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

                faqPages.Add(new FaqPageViewModel
                {
                    FileName = Path.GetFileNameWithoutExtension(file),
                    Sections = sections ?? new List<FaqSection>()
                });
            }

            return View(faqPages);
        }
    }
}