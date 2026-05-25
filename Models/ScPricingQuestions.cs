using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CSMS.Models
{
    [Table("SC_Pricing_Questions")]
    public partial class ScPricingQuestions
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string DutyStation { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(10)]
        public string SurveyMonth { get; set; }
        [StringLength(10)]
        public string SurveyYear { get; set; }
        [StringLength(6)]
        public string ItemCode { get; set; }
        [StringLength(4)]
        public string StoreId { get; set; }
        [StringLength(150)]
        public string Brand { get; set; }
        public double? Quantity { get; set; }
        [StringLength(10)]
        public string Unit { get; set; }
        public double? Price { get; set; }
        [StringLength(750)]
        public string Comments { get; set; }
        public double? BasicChargeUnit { get; set; }
        public double? BasicChargeRate { get; set; }
        public double? AdditionalChargeUnit { get; set; }
        public double? AdditionalChargeRate { get; set; }
        public double? OtherChargeUnit { get; set; }
        public double? OtherChargeRate { get; set; }
        public double? ThirdParty { get; set; }
        public double? Comprehensive { get; set; }
        public double? Collision { get; set; }
        public double? RegistrationFee { get; set; }
        [StringLength(13)]
        public string Syscode { get; set; }
    }
}
