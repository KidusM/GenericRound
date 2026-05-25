using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CSMS.Models
{
    public partial class Survey
    {
        public Survey()
        {
            ScreportQuestions = new HashSet<ScreportQuestion>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [Column("DSID")]
        [StringLength(450)]
        [DisplayName("DS Code")]
        public string Dsid { get; set; }
        [StringLength(10)]
        [DisplayName("Survey Type")]
        [Required]
        public string SurveyType { get; set; }
        [StringLength(10)]
        [Required]
        public string Round { get; set; }
        [StringLength(10)]
        [DisplayName("Month")]
        [Required]
        public string SurveyMonth { get; set; }
        [StringLength(10)]
        [DisplayName("Year")]
        [Required]
        public string SurveyYear { get; set; }
        [Column(TypeName = "date")]
        [DisplayName("Start")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yy}", ApplyFormatInEditMode = false)]
        public DateTime? SurveyBegin { get; set; }
        [Column(TypeName = "date")]
        [DisplayName("End")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yy}", ApplyFormatInEditMode = false)]
        public DateTime? SurveyEnd { get; set; }
        [DisplayName("No. of Staff")]
        public int? NoOfStaff { get; set; }
        [StringLength(13)]
        public string Syscode { get; set; }


        [Column("SCId")]
        [StringLength(450)]
        [DisplayName("Survey Coordinator")]
        public string Scid { get; set; }

        [Column("SMId")]
        [StringLength(450)]
        [DisplayName("ICSC Focal Person")]
        public string Smid { get; set; }

        [ForeignKey(nameof(Dsid))]
        [InverseProperty(nameof(DutyStation.Surveys))]
        [DisplayName("Duty Station")]
        public virtual DutyStation Ds { get; set; }

        [ForeignKey(nameof(Scid))]
        [InverseProperty(nameof(ApplicationUser.SurveyScs))]
        [DisplayName("Survey Coordinator")]
        public virtual ApplicationUser Sc { get; set; }


        [ForeignKey(nameof(Smid))]
        [InverseProperty(nameof(ApplicationUser.SurveySms))]
        [DisplayName("ICSC Focal Person")]
        public virtual ApplicationUser Sm { get; set; }
        [InverseProperty(nameof(ScreportQuestion.Survey))]
        public virtual ICollection<ScreportQuestion> ScreportQuestions { get; set; }

        
    }
}
