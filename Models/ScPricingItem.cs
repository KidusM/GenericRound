using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CSMS.Models
{
    [Table("SC_Pricing_Item")]
    public partial class ScPricingItem
    {
        public ScPricingItem()
        {
            ScPricingData = new HashSet<ScPricingData>();
        }

        [Key]
        [Column("ITEM_CODE")]
        [StringLength(6)]
        public string ItemCode { get; set; }
        [Column("ITEM_NAME")]
        [StringLength(100)]
        public string ItemName { get; set; }
        [Column("ITEM_DESCRIPTION")]
        [StringLength(750)]
        public string ItemDescription { get; set; }
        [Column("ITEM_CATEGORY")]
        [StringLength(50)]
        public string ItemCategory { get; set; }
        [Column("DEFAULT_UNIT")]
        [StringLength(15)]
        public string DefaultUnit { get; set; }

        [InverseProperty("ItemCodeNavigation")]
        public virtual ICollection<ScPricingData> ScPricingData { get; set; }
    }
}
