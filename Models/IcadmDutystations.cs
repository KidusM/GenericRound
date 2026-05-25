using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CSMS.Models
{
    [Table("ICADM_DUTYSTATIONS")]
    public partial class IcadmDutystations
    {
        [Key]
        [Column("DS_ID")]
        [StringLength(6)]
        public string DsId { get; set; }
        [Required]
        [Column("DS_NAME")]
        [StringLength(50)]
        public string DsName { get; set; }
        [Required]
        [Column("DS_NO")]
        [StringLength(3)]
        public string DsNo { get; set; }
        [Column("COUNTRY_ID")]
        [StringLength(3)]
        public string CountryId { get; set; }
        [Column("REGION_ID")]
        [StringLength(3)]
        public string RegionId { get; set; }

        [ForeignKey(nameof(CountryId))]
        [InverseProperty(nameof(IcadmCountry.IcadmDutystations))]
        public virtual IcadmCountry Country { get; set; }
        [ForeignKey(nameof(RegionId))]
        [InverseProperty(nameof(IcadmRegion.IcadmDutystations))]
        public virtual IcadmRegion Region { get; set; }
    }
}
