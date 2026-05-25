using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CSMS.Models
{
    [Table("Item_Store_Price")]
    public partial class ItemStorePrice
    {
        [Key]
        [Column("ITEM_CODE")]
        [StringLength(6)]
        public string ItemCode { get; set; }
        [Key]
        [Column("SYSCODE")]
        [StringLength(13)]
        public string Syscode { get; set; }
        [Key]
        [Column("STORE_ID")]
        [StringLength(4)]
        public string StoreId { get; set; }
        [Column("BRAND")]
        [StringLength(75)]
        public string Brand { get; set; }
        [Column("UNIT_USED")]
        [StringLength(3)]
        public string UnitUsed { get; set; }
        [Column("REGULAR_PRICE")]
        [StringLength(20)]
        public string RegularPrice { get; set; }
        [Column("COMMENT")]
        [StringLength(75)]
        public string Comment { get; set; }
        [Column("Organic_Flag")]
        [StringLength(1)]
        public string OrganicFlag { get; set; }
        [Column("OBSERVATION")]
        [StringLength(75)]
        public string Observation { get; set; }
        [Column("SALE_DISCOUNT")]
        [StringLength(1)]
        public string SaleDiscount { get; set; }
        [Column("QUANTITY", TypeName = "decimal(12, 4)")]
        public decimal? Quantity { get; set; }
        [Column("COST_LOCAL", TypeName = "decimal(22, 6)")]
        public decimal? CostLocal { get; set; }

        [ForeignKey(nameof(ItemCode))]
        public virtual ScPricingItem ItemCodeNavigation { get; set; }
        [ForeignKey("Syscode,StoreId")]
        public virtual ScPricingStores S { get; set; }
       // public List<ItemStorePrice> ItemStorePricesa { get; set; }
    }
}
