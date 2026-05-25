using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CSMS.Models
{
    [Table("ICADM_COUNTRY")]
    public partial class IcadmCountry
    {
        public IcadmCountry()
        {
            IcadmDutystations = new HashSet<IcadmDutystations>();
            IccdmDutystations = new HashSet<IccdmDutystations>();
        }

        [Key]
        [Column("COUNTRY_ID")]
        [StringLength(3)]
        public string CountryId { get; set; }
        [Column("COUNTRY_NAME")]
        [StringLength(50)]
        public string CountryName { get; set; }
        [Column("REGION_ID")]
        [StringLength(3)]
        public string RegionId { get; set; }
        [Required]
        [Column("CNT_VALID")]
        [StringLength(1)]
        public string CntValid { get; set; }
        [Column("CNT_MEDICAL")]
        public bool? CntMedical { get; set; }

        [ForeignKey(nameof(RegionId))]
        [InverseProperty(nameof(IcadmRegion.IcadmCountry))]
        public virtual IcadmRegion Region { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<IcadmDutystations> IcadmDutystations { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<IccdmDutystations> IccdmDutystations { get; set; }
    }
}
