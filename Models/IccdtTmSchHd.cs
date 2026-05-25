using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CSMS.Models
{
    [Table("ICCDT_TM_SCH_HD")]
    public partial class IccdtTmSchHd
    {
        [Key]
        [Column("SYSCODE")]
        [StringLength(13)]
        public string Syscode { get; set; }
        [Required]
        [Column("DS_ID")]
        [StringLength(6)]
        public string DsId { get; set; }
        [Required]
        [Column("SURVEY_TYPE")]
        [StringLength(1)]
        public string SurveyType { get; set; }
        [Column("PROPOSED_YEAR")]
        public short ProposedYear { get; set; }
        [Column("PROPOSED_MONTH")]
        public byte ProposedMonth { get; set; }
        [Column("ACTUAL_YEAR")]
        public short? ActualYear { get; set; }
        [Column("ACTUAL_MONTH")]
        public byte? ActualMonth { get; set; }
        [Column("NO_CCAQ")]
        public short? NoCcaq { get; set; }
        [Column("LEAD_AGENCY")]
        [StringLength(30)]
        public string LeadAgency { get; set; }
        [Column("PERSONNEL_STAFF")]
        public short? PersonnelStaff { get; set; }
        [Column("RESPONSE", TypeName = "decimal(9, 2)")]
        public decimal? Response { get; set; }
        [Column("RESPONSE_RATE", TypeName = "decimal(9, 2)")]
        public decimal? ResponseRate { get; set; }
        [Column("DATE_VERIFIED", TypeName = "datetime")]
        public DateTime? DateVerified { get; set; }
        [Column("DATE_UPDATED", TypeName = "datetime")]
        public DateTime? DateUpdated { get; set; }
        [Column("DATE_RESULTS", TypeName = "datetime")]
        public DateTime? DateResults { get; set; }
        [Column("DATE_IMPLEMENTED", TypeName = "datetime")]
        public DateTime? DateImplemented { get; set; }
        [Column("DATE_REPORTED", TypeName = "datetime")]
        public DateTime? DateReported { get; set; }
        [Column("FOLLOWUP")]
        [StringLength(1)]
        public string Followup { get; set; }
        [Column("FOLLOWUP_COMMENTS")]
        [StringLength(400)]
        public string FollowupComments { get; set; }
        [Column("CANCELLED")]
        [StringLength(1)]
        public string Cancelled { get; set; }
        [Column("DATA_ENTRY_FLAG")]
        [StringLength(1)]
        public string DataEntryFlag { get; set; }
        [Column("VERIFY_FLAG")]
        [StringLength(1)]
        public string VerifyFlag { get; set; }
        [Column("UPDATION_FLAG")]
        [StringLength(1)]
        public string UpdationFlag { get; set; }
        [Column("DATE_CV", TypeName = "datetime")]
        public DateTime? DateCv { get; set; }
        [Column("SALARY", TypeName = "decimal(19, 6)")]
        public decimal? Salary { get; set; }
        [Column("EX_RATE", TypeName = "decimal(14, 6)")]
        public decimal? ExRate { get; set; }
        [Column("INDICATIVE_FEES", TypeName = "decimal(24, 6)")]
        public decimal? IndicativeFees { get; set; }
        [Column("hh_mover")]
        public int? HhMover { get; set; }
        [Column("DATE_WEB_START", TypeName = "datetime")]
        public DateTime? DateWebStart { get; set; }
        [Column("DATE_WEB_END", TypeName = "datetime")]
        public DateTime? DateWebEnd { get; set; }
        [Column("FS_POSITIONS")]
        [StringLength(1)]
        public string FsPositions { get; set; }

        [ForeignKey(nameof(DsId))]
        [InverseProperty(nameof(IccdmDutystations.IccdtTmSchHd))]
        public virtual IccdmDutystations Ds { get; set; }
    }
}
