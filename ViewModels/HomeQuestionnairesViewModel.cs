using CSMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMS.ViewModels
{
    public class HomeQuestionnairesViewModel
    {
        public Survey survey { get; set; }
        public string Dsid { get; set; }
        public int ServeyId { get; set; }
        public string Scid { get; set; }
        public string Smid { get; set; }
    }
}
