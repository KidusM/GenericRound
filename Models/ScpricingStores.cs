using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CSMS.Models
{
    [Table("SC_Pricing_Stores")]
    public partial class ScPricingStores
    {
        public ScPricingStores()
        {
            ScPricingQuestionnaires = new HashSet<ScPricingQuestionnaire>();
        }

        [Column("DS_ID")]
        [StringLength(6)]
        public string DsId { get; set; }
        [Key]
        [Column("SYSCODE")]
        [StringLength(13)]
        public string Syscode { get; set; }
        [Key]
        [Required]
        [Column("STORES_ID")]
        [DisplayName("Outlet No.")]
        [StringLength(4)]
        public string StoresId { get; set; }
        [Column("ACTUAL_YEAR")]
        public short? ActualYear { get; set; }
        [Column("ACTUAL_MONTH")]
        public byte? ActualMonth { get; set; }
        [Column("STORES_NAME")]
        [DisplayName("Outlet Name")]
        [StringLength(75)]
        public string StoresName { get; set; }
        [Column("STORES_DESCRIPTION")]
        [DisplayName("Outlet Description")]
        [StringLength(150)]
        public string StoresDescription { get; set; }
        [Column("EXCLUDE")]
        [StringLength(1)]
        public string Exclude { get; set; }
        [Column("Ex_remarks")]
        [StringLength(50)]
        public string ExRemarks { get; set; }
        [StringLength(1)]
        public string Internet { get; set; }
        [Column("STORE_ADDRESS")]
        [DisplayName("Physical Addres")]
        [StringLength(500)]
        public string StoreAddress { get; set; }
        [Column("WEB_ADDRESS")]
        [DisplayName("Website (URL)")]
        [StringLength(200)]
        public string WebAddress { get; set; }
        [Column("GPS_COORDINATES")]
        [DisplayName("GPS Coordinates")]
        [StringLength(500)]
        public string GpsCoordinates { get; set; }
        [Column("OUTLET_TYPE_ID")]
        public short? OutletTypeId { get; set; }

        //[InverseProperty(nameof(ScPricingQuestionnaire.S))]
        public virtual ICollection<ScPricingQuestionnaire> ScPricingQuestionnaires { get; set; }
    }
}
