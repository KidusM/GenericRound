using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CSMS.Models
{
    public partial class DutyStation
    {
        public DutyStation()
        {
            Surveys = new HashSet<Survey>();
        }

        [Key]
        [Column("DSID")]
        [DisplayName("DS Code")]
        public string Dsid { get; set; }
        [StringLength(50)]
        public string Country { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [Column("DSGroup")]
        [StringLength(10)]
        [DisplayName("DS Group")]
        public string Dsgroup { get; set; }

        [InverseProperty(nameof(Survey.Ds))]
        [DisplayName("Duty Station")]
        public virtual ICollection<Survey> Surveys { get; set; }
    }
}
