using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CSMS.Models
{
    [Table("ICADM_REGION")]
    public partial class IcadmRegion
    {
        public IcadmRegion()
        {
            IcadmCountry = new HashSet<IcadmCountry>();
            IcadmDutystations = new HashSet<IcadmDutystations>();
        }

        [Key]
        [Column("REGION_ID")]
        [StringLength(3)]
        public string RegionId { get; set; }
        [Required]
        [Column("REG_NAME")]
        [StringLength(50)]
        public string RegName { get; set; }

        [InverseProperty("Region")]
        public virtual ICollection<IcadmCountry> IcadmCountry { get; set; }
        [InverseProperty("Region")]
        public virtual ICollection<IcadmDutystations> IcadmDutystations { get; set; }
    }
}
