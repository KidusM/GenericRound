using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CSMS.Models
{
    [Table("Survey_Store")]
    public partial class SurveyStore
    {
        [Key]
        [Column("PricingQId")]
        public int PricingQid { get; set; }
        [Key]
        public int StoreId { get; set; }

        [ForeignKey(nameof(PricingQid))]
        [InverseProperty(nameof(ScPricingData.SurveyStore))]
        public virtual ScPricingData PricingQ { get; set; }
        [ForeignKey(nameof(StoreId))]
        [InverseProperty(nameof(ScStores.SurveyStore))]
        public virtual ScStores Store { get; set; }
    }
}
