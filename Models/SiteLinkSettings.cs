using System.Collections.Generic;

namespace CSMS.Models
{
    public class SiteLinkSettings
    {
        public Dictionary<string, string> SurveyUrls { get; set; }
            = new Dictionary<string, string>();

        public string PAManualUrl { get; set; }

        public Dictionary<string, string> ElectroniDiaryEnglishURL { get; set; }
            = new Dictionary<string, string>();

        public Dictionary<string, string> ElectroniDiaryFrenchURL { get; set; }
            = new Dictionary<string, string>();
    }
}