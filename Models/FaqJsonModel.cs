using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CSMS.Models
{
    public class FaqPageViewModel
    {
        public string FileName { get; set; }
        public List<FaqSection> Sections { get; set; } = new List<FaqSection>();
    }

    public class FaqSection
    {
        public string Type { get; set; }
        public List<FaqItem> Items { get; set; } = new List<FaqItem>();
    }

    public class FaqItem
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}