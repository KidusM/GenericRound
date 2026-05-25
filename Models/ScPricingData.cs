using Microsoft.AspNetCore.Http;
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
    [Table("SC_Pricing_Data")]
    public partial class ScPricingData
    {
        public ScPricingData()
        {
            SurveyStore = new HashSet<SurveyStore>();
        }

        [Key]
        public int Id { get; set; }
        [DisplayName("Duty Station")]
        [StringLength(50)]
        [Required]
        public string DutyStation { get; set; }
        [StringLength(50)]
        [Required]
        public string City { get; set; }
        [DisplayName("Survey Month")]
        [StringLength(10)]
        [Required]
        public string SurveyMonth { get; set; }
        [DisplayName("Survey Year")]
        [StringLength(10)]
        [Required]
        public string SurveyYear { get; set; }
        [StringLength(6)]
        public string ItemCode { get; set; }
        [Required]
        [DisplayName("Store Id")]
        public int? StoreId { get; set; }
        [StringLength(150)]
        public string Brand { get; set; }
        [Required]
        public double? Quantity { get; set; }
        [Required]
        [StringLength(10)]
        public string Unit { get; set; }
        [Required]
        public double? Price { get; set; }
        [StringLength(750)]
        public string Comments { get; set; }
        [DisplayName("Basic Charge")]
        [StringLength(150)]
        public string BasicChargeUnit { get; set; }
        [DisplayName("Basic Charge Rate")]
        [StringLength(150)]
        public string BasicChargeRate { get; set; }
        [DisplayName("Additional Charge Unit")]
        [StringLength(150)]
        public string AdditionalChargeUnit { get; set; }
        [DisplayName("Additional Unit Rate")]
        [StringLength(150)]
        public string AdditionalChargeRate { get; set; }
        [DisplayName("Other Charge Unit")]
        [StringLength(150)]
        public string OtherChargeUnit { get; set; }
        [DisplayName("Other Charge Rate")]
        [StringLength(150)]
        public string OtherChargeRate { get; set; }
        [DisplayName("Third Party")]
        public double? ThirdParty { get; set; }
        [DisplayName("Comprehensive")]
        public double? Comprehensive { get; set; }
        [DisplayName("Collision")]
        public double? Collision { get; set; }
        [DisplayName("Annual Registration")]
        public double? RegistrationFee { get; set; }
        [StringLength(13)]
        public string Syscode { get; set; }

        [ForeignKey(nameof(ItemCode))]
        [InverseProperty(nameof(ScPricingItem.ScPricingData))]
        public virtual ScPricingItem ItemCodeNavigation { get; set; }
        [InverseProperty("PricingQ")]
        public virtual ICollection<SurveyStore> SurveyStore { get; set; }

    }
}
