using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CSMS.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
                SurveyScs = new HashSet<Survey>();
                SurveySms = new HashSet<Survey>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [InverseProperty(nameof(Survey.Sc))]
        public virtual ICollection<Survey> SurveyScs { get; set; }

        [InverseProperty(nameof(Survey.Sm))]
        public virtual ICollection<Survey> SurveySms { get; set; }
    }
}
