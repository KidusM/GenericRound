using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CSMS.Models
{
    public class OfficeActivity
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public List<ActivityStep> Steps { get; set; } = new List<ActivityStep>();
    }

    public class ActivityStep
    {
        public int StepNumber { get; set; }

        public string StepTitle { get; set; }

        public string StepDetails { get; set; }

        public string ScreenshotPath { get; set; }

        public string ScreenshotBase64 { get; set; }
    }
}