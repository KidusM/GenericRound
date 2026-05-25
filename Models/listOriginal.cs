using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CSMS.Models
{
    [Table("cld_listOriginal")]
    public partial class listOriginal
    {
       
        [Key]
        [Column("idno")]
        [DisplayName("idno")]
        public int? idno { get; set; }
        public string dsid { get; set; }
        public string survdate { get; set; }
        public int? Groupno { get; set; }
        public int? Roundno { get; set; }
        public int? survyear { get; set; }
        public int? staffno { get; set; }
        public string survtype { get; set; }
        public string DutyStationId { get; set; }
        public string City { get; set; }
        [Column(TypeName = "date")]
        [DisplayName("Start")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yy}", ApplyFormatInEditMode = false)]
        public DateTime? SurveyBegin { get; set; }
        [Column(TypeName = "date")]
        [DisplayName("End")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yy}", ApplyFormatInEditMode = false)]
        public DateTime? SurveyEnd { get; set; }

    }
}
