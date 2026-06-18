using System.Collections.Generic;

namespace CSMS.Models
{
    public class OfficeInstructionDivision
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public List<OfficeActivity> Activities { get; set; } = new List<OfficeActivity>();
    }
}