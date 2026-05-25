using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CSMS.Models
{
    [Table("SC_Report")]
    public partial class ScReport
    {
        public int Id { get; set; }
        public int? SurveyId { get; set; }
        public bool CommercialOutSide { get; set; }
        public bool CommercialInSide { get; set; }
        public bool GovtOutSide { get; set; }
        public bool GovtInSide { get; set; }
        [Column("UNOutSide")]
        public bool UnoutSide { get; set; }
        [Column("UNInSide")]
        public bool UninSide { get; set; }
        public bool OtherHousingOutside { get; set; }
        public bool OtherHousingInside { get; set; }
        public string OtherHousing { get; set; }
        public double? OverallQuality { get; set; }
        public string OverallQualityComments { get; set; }
        public double? Availability { get; set; }
        public string AvailabilityComments { get; set; }
        public double? MaintenanceLevel { get; set; }
        public string MaintenanceComments { get; set; }
        public double? TimeForNewComer { get; set; }
        public string TimeForNewComerComments { get; set; }
        [Column("KeyMoney_Security_Deposit")]
        public string KeyMoneySecurityDeposit { get; set; }
        public bool AdditionalSecurityCost { get; set; }
        public bool SecurityCostReimbursed { get; set; }
        public bool? FullyReimbursedAllAgencies { get; set; }
        public bool? FullyReimbursedSomeAgencies { get; set; }
        public bool? PartiallyReimbursedAllAgencies { get; set; }
        public bool? PartiallyReimbursedSomeAgencies { get; set; }
        public string AgenciesProvidingReimbursement { get; set; }
        public string AlarmInstallationFrequency { get; set; }
        public string AlarmInstallationPercentage { get; set; }
        public double? AlarmInstallationMaxAmount { get; set; }
        public string AlarmInstallationCurrency { get; set; }
        public string AlarmInstallationEffectiveDate { get; set; }
        public string AlarmInstallationComments { get; set; }
        public string HiringAlarmSystemFrequency { get; set; }
        public string HiringAlarmSystemPercentage { get; set; }
        public double? HiringAlarmSystemMaxAmount { get; set; }
        public string HiringAlarmSystemCurrency { get; set; }
        [Column(TypeName = "date")]
        public DateTime? HiringAlarmSystemEffectiveDate { get; set; }
        public string HiringAlarmSystemComments { get; set; }
        public string BarsFrequency { get; set; }
        public string BarsPercentage { get; set; }
        public double? BarsMaxAmount { get; set; }
        public string BarsCurrency { get; set; }
        [Column(TypeName = "date")]
        public DateTime? BarsEffectiveDate { get; set; }
        public string BarsComments { get; set; }
        public string GuardsFrequency { get; set; }
        public string GuardsPercentage { get; set; }
        public double? GuardsMaxAmount { get; set; }
        public string GuardsCurrency { get; set; }
        [Column(TypeName = "date")]
        public DateTime? GuardsEffectiveDate { get; set; }
        public string GuardsComments { get; set; }
        public string Other1Frequency { get; set; }
        public string Other1Percentage { get; set; }
        public double? Other1MaxAmount { get; set; }
        public string Other1Currency { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Other1EffectiveDate { get; set; }
        public string Other1Comments { get; set; }
        public string Other2Frequency { get; set; }
        public string Other2Percentage { get; set; }
        public double? Other2MaxAmount { get; set; }
        public string Other2Currency { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Other2EffectiveDate { get; set; }
        public string Other2Comments { get; set; }
        public bool CommonToEmployDomestic { get; set; }
        public bool SocialInsuranceExists { get; set; }
        public bool SocialInsuranceApplyToDomestic { get; set; }
        public double? EmployerContributionAmount { get; set; }
        public string EmployerContributionCurrency { get; set; }
        public string DomesticHelpComment { get; set; }
        public bool PurchaseUsingForeignCurrency { get; set; }
        public string CurrencyName { get; set; }
        public string ListCurrencies { get; set; }
        public double? TaxAddedAtPurchase { get; set; }
        public string Commodity1 { get; set; }
        public string TaxCommodity1 { get; set; }
        public string Commodity2 { get; set; }
        public string TaxCommodity2 { get; set; }
        public string Commodity3 { get; set; }
        public string TaxCommodity3 { get; set; }
        public string Commodity4 { get; set; }
        public string TaxCommodity4 { get; set; }
        public string Commodity5 { get; set; }
        public string TaxCommodity5 { get; set; }
        public string Commodity6 { get; set; }
        public string TaxCommodity6 { get; set; }
        public string Commodity7 { get; set; }
        public string TaxCommodity7 { get; set; }
        public string Commodity8 { get; set; }
        public string TaxCommodity8 { get; set; }
        public string Commodity9 { get; set; }
        public string TaxCommodity9 { get; set; }
        public string Commodity10 { get; set; }
        public string TaxCommodity10 { get; set; }
        public string SurveyMonth { get; set; }
        public string SurveyYear { get; set; }
        public string SurveyType { get; set; }
        [Key]
        [StringLength(13)]
        public string Syscode { get; set; }
    }
}
