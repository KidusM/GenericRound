using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CSMS.Models
{
    [Table("ICCDM_DUTYSTATIONS")]
    public partial class IccdmDutystations
    {
        public IccdmDutystations()
        {
            IccdtTmSchHd = new HashSet<IccdtTmSchHd>();
        }

        [Column("COUNTRY_ID")]
        [StringLength(3)]
        public string CountryId { get; set; }
        [Key]
        [Column("DS_ID")]
        [StringLength(6)]
        public string DsId { get; set; }
        [Column("DS_NAME")]
        [StringLength(50)]
        public string DsName { get; set; }
        [Column("HQ_FLAG")]
        [StringLength(1)]
        public string HqFlag { get; set; }
        [Required]
        [Column("DS_GROUP")]
        [StringLength(1)]
        public string DsGroup { get; set; }
        [Column("BASE_DS")]
        [StringLength(6)]
        public string BaseDs { get; set; }
        [Column("BASE_PLACE")]
        [StringLength(50)]
        public string BasePlace { get; set; }
        [Column("BASE_DATE", TypeName = "datetime")]
        public DateTime? BaseDate { get; set; }
        [Column("LINK_DS")]
        [StringLength(6)]
        public string LinkDs { get; set; }
        [Column("INTERNAL_DS")]
        [StringLength(1)]
        public string InternalDs { get; set; }
        [Column("COEFFICIENT", TypeName = "decimal(12, 6)")]
        public decimal? Coefficient { get; set; }
        [Column("cpi_ds")]
        [StringLength(6)]
        public string CpiDs { get; set; }
        [Column("active_flag")]
        [StringLength(1)]
        public string ActiveFlag { get; set; }

        [ForeignKey(nameof(CountryId))]
        [InverseProperty(nameof(IcadmCountry.IccdmDutystations))]
        public virtual IcadmCountry Country { get; set; }
        [InverseProperty("Ds")]
        public virtual ICollection<IccdtTmSchHd> IccdtTmSchHd { get; set; }
    }
}
