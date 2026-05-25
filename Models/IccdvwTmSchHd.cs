using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CSMS.Models
{

        public partial class IccdvwTmSchHd
        { 
            //public Guid Id { get; set; } = Guid.NewGuid();
            [Required]
            [Column("SYSCODE")]
            [StringLength(13)]
            public string Syscode { get; set; }
            [Required]
            [Column("DS_ID")]
            [DisplayName("DS Code")]
            [StringLength(6)]
            public string DsId { get; set; }
            [Required]
            [Column("DS_NAME")]
            [DisplayName("City")]
            [StringLength(50)]
            public string DsName { get; set; }
            [Column("COUNTRY_NAME")]
        [DisplayName("Country")]
        [StringLength(50)]
            public string CountryName { get; set; }
            [Column("ACTUAL_YEAR")]
            public short? ActualYear { get; set; }
            [Column("ACTUAL_MONTH")]
            public byte? ActualMonth { get; set; }
            [Column("PROPOSED_YEAR")]
        [DisplayName("Proposed Year")]
        public short ProposedYear { get; set; }
            [Column("PROPOSED_MONTH")]
            [DisplayName("Proposed Month")]
            public byte ProposedMonth { get; set; }
            [Column("DATE_WEB_START", TypeName = "datetime")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yy}", ApplyFormatInEditMode = false)]
        [DisplayName("Start Date")]
        public DateTime? DateWebStart { get; set; }
            [Column("DATE_WEB_END", TypeName = "datetime")]
        [DisplayName("End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yy}", ApplyFormatInEditMode = false)]
        public DateTime? DateWebEnd { get; set; }
            [Required]
            [Column("CURRENCY_ID")]
            [StringLength(3)]
            public string CurrencyId { get; set; }
            [Column("EX_RATE", TypeName = "decimal(20, 6)")]
            public decimal ExRate { get; set; }
            [Column("ACTUAL_DATE", TypeName = "datetime")]
            public DateTime ActualDate { get; set; }
            [Column("COUNTRY_ID")]
            [StringLength(3)]
            public string CountryId { get; set; }
            [Required]
            [Column("SURVEY_TYPE")]
            [StringLength(1)]
            public string SurveyType { get; set; }
            [Column("FS_POSITIONS")]
            [StringLength(1)]
            public string FsPositions { get; set; }
        }
 }
