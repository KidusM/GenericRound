using CSMS.Models;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CSMS.Services
{
    public class SiteLinkService
    {
        private readonly IWebHostEnvironment _environment;

        public SiteLinkService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        private string GetFilePath()
        {
            var folderPath = Path.Combine(
                _environment.ContentRootPath,
                "App_Data");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return Path.Combine(folderPath, "SiteLinks.json");
        }

        public SiteLinkSettings GetLinks()
        {
            var filePath = GetFilePath();

            if (!File.Exists(filePath))
            {
                var defaultSettings = new SiteLinkSettings
                {
                    SurveyUrls = new Dictionary<string, string>(),
                    PAManualUrl = "",
                    ElectroniDiaryEnglishURL = new Dictionary<string, string>(),
                    ElectroniDiaryFrenchURL = new Dictionary<string, string>()
                };

                SaveLinks(defaultSettings);
                return defaultSettings;
            }

            var json = File.ReadAllText(filePath);
            var settings = JsonSerializer.Deserialize<SiteLinkSettings>(json);

            if (settings == null)
            {
                settings = new SiteLinkSettings();
            }

            if (settings.SurveyUrls == null)
            {
                settings.SurveyUrls = new Dictionary<string, string>();
            }

            if (settings.ElectroniDiaryEnglishURL == null)
            {
                settings.ElectroniDiaryEnglishURL =
                    new Dictionary<string, string>();
            }

            if (settings.ElectroniDiaryFrenchURL == null)
            {
                settings.ElectroniDiaryFrenchURL =
                    new Dictionary<string, string>();
            }

            return settings;
        }

        public void SaveLinks(SiteLinkSettings settings)
        {
            var filePath = GetFilePath();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json =
                JsonSerializer.Serialize(settings, options);

            File.WriteAllText(filePath, json);
        }

        public SiteLinkSettings EnsureSurveyRounds(
    IEnumerable<string> surveyRounds)
        {
            var settings = GetLinks();
            var changed = false;

            foreach (var round in surveyRounds)
            {
                if (string.IsNullOrWhiteSpace(round))
                {
                    continue;
                }

                if (!settings.SurveyUrls.ContainsKey(round))
                {
                    settings.SurveyUrls.Add(round, "");
                    changed = true;
                }

                if (!settings.ElectroniDiaryEnglishURL.ContainsKey(round))
                {
                    settings.ElectroniDiaryEnglishURL.Add(round, "");
                    changed = true;
                }

                if (!settings.ElectroniDiaryFrenchURL.ContainsKey(round))
                {
                    settings.ElectroniDiaryFrenchURL.Add(round, "");
                    changed = true;
                }
            }

            if (changed)
            {
                SaveLinks(settings);
            }

            return settings;
        }

        public string GetElectronicDiaryEnglishUrl(string surveyRound)
        {
            var settings = GetLinks();

            if (string.IsNullOrWhiteSpace(surveyRound))
            {
                return "";
            }

            if (settings.ElectroniDiaryEnglishURL.TryGetValue(
                surveyRound,
                out var url))
            {
                return url ?? "";
            }

            return "";
        }

        public string GetElectronicDiaryFrenchUrl(string surveyRound)
        {
            var settings = GetLinks();

            if (string.IsNullOrWhiteSpace(surveyRound))
            {
                return "";
            }

            if (settings.ElectroniDiaryFrenchURL.TryGetValue(
                surveyRound,
                out var url))
            {
                return url ?? "";
            }

            return "";
        }

        public string GetSurveyUrl(string surveyRound)
        {
            var settings = GetLinks();

            if (string.IsNullOrWhiteSpace(surveyRound))
            {
                return "";
            }

            if (settings.SurveyUrls.TryGetValue(
                surveyRound,
                out var url))
            {
                return url ?? "";
            }

            return "";
        }
    }
}