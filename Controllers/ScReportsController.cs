using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CSMS.Data;
using CSMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace CSMS.Controllers
{
    [Authorize]
    public class ScReportsController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        public ScReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ScReports
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScReport.ToListAsync());
        }

        // GET: ScReports/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scReport = await _context.ScReport
                .FirstOrDefaultAsync(m => m.Syscode == id);
            if (scReport == null)
            {
                return NotFound();
            }

            return View(scReport);
        }

        // GET: ScReports/Create
        public IActionResult Create()
        {
            var mySyscode = HttpContext.Session.GetString("_Syscode");

            HttpContext.Session.SetString("_SCReportCompleted", "Incomplete");
            var existing = _context.ScReport.Where(x=>x.Syscode == mySyscode).FirstOrDefault();
            if (existing != null)
            {
                HttpContext.Session.SetString("_SCReportCompleted", "Complete");
            }
            return View();
        }

        // POST: ScReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SurveyId,CommercialOutSide,CommercialInSide,GovtOutSide,GovtInSide,UnoutSide,UninSide,OtherHousingOutside,OtherHousingInside,OtherHousing,OverallQuality,OverallQualityComments,Availability,AvailabilityComments,MaintenanceLevel,MaintenanceComments,TimeForNewComer,TimeForNewComerComments,KeyMoneySecurityDeposit,AdditionalSecurityCost,SecurityCostReimbursed,FullyReimbursedAllAgencies,FullyReimbursedSomeAgencies,PartiallyReimbursedAllAgencies,PartiallyReimbursedSomeAgencies,AgenciesProvidingReimbursement,AlarmInstallationFrequency,AlarmInstallationPercentage,AlarmInstallationMaxAmount,AlarmInstallationCurrency,AlarmInstallationEffectiveDate,AlarmInstallationComments,HiringAlarmSystemFrequency,HiringAlarmSystemPercentage,HiringAlarmSystemMaxAmount,HiringAlarmSystemCurrency,HiringAlarmSystemEffectiveDate,HiringAlarmSystemComments,BarsFrequency,BarsPercentage,BarsMaxAmount,BarsCurrency,BarsEffectiveDate,BarsComments,GuardsFrequency,GuardsPercentage,GuardsMaxAmount,GuardsCurrency,GuardsEffectiveDate,GuardsComments,Other1Frequency,Other1Percentage,Other1MaxAmount,Other1Currency,Other1EffectiveDate,Other1Comments,Other2Frequency,Other2Percentage,Other2MaxAmount,Other2Currency,Other2EffectiveDate,Other2Comments,CommonToEmployDomestic,SocialInsuranceExists,SocialInsuranceApplyToDomestic,EmployerContributionAmount,EmployerContributionCurrency,DomesticHelpComment,PurchaseUsingForeignCurrency,CurrencyName,ListCurrencies,TaxAddedAtPurchase,Commodity1,TaxCommodity1,Commodity2,TaxCommodity2,Commodity3,TaxCommodity3,Commodity4,TaxCommodity4,Commodity5,TaxCommodity5,Commodity6,TaxCommodity6,Commodity7,TaxCommodity7,Commodity8,TaxCommodity8,Commodity9,TaxCommodity9,Commodity10,TaxCommodity10,SurveyMonth,SurveyYear,SurveyType,Syscode")] ScReport scReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }
            return View(scReport);
        }

        // GET: ScReports/Edit/5
        public async Task<IActionResult> Edit(string syscode, int id)
        {
            if (syscode == null)
            {
                return NotFound();
            }

            var scReport = await _context.ScReport.FindAsync(syscode);
            if (scReport == null)
            {
                return NotFound();
            }
            return View(scReport);
        }

        // POST: ScReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string syscode, int id, [Bind("Syscode,SurveyId,CommercialOutSide,CommercialInSide,GovtOutSide,GovtInSide,UnoutSide,UninSide,OtherHousingOutside,OtherHousingInside,OtherHousing,OverallQuality,OverallQualityComments,Availability,AvailabilityComments,MaintenanceLevel,MaintenanceComments,TimeForNewComer,TimeForNewComerComments,KeyMoneySecurityDeposit,AdditionalSecurityCost,SecurityCostReimbursed,FullyReimbursedAllAgencies,FullyReimbursedSomeAgencies,PartiallyReimbursedAllAgencies,PartiallyReimbursedSomeAgencies,AgenciesProvidingReimbursement,AlarmInstallationFrequency,AlarmInstallationPercentage,AlarmInstallationMaxAmount,AlarmInstallationCurrency,AlarmInstallationEffectiveDate,AlarmInstallationComments,HiringAlarmSystemFrequency,HiringAlarmSystemPercentage,HiringAlarmSystemMaxAmount,HiringAlarmSystemCurrency,HiringAlarmSystemEffectiveDate,HiringAlarmSystemComments,BarsFrequency,BarsPercentage,BarsMaxAmount,BarsCurrency,BarsEffectiveDate,BarsComments,GuardsFrequency,GuardsPercentage,GuardsMaxAmount,GuardsCurrency,GuardsEffectiveDate,GuardsComments,Other1Frequency,Other1Percentage,Other1MaxAmount,Other1Currency,Other1EffectiveDate,Other1Comments,Other2Frequency,Other2Percentage,Other2MaxAmount,Other2Currency,Other2EffectiveDate,Other2Comments,CommonToEmployDomestic,SocialInsuranceExists,SocialInsuranceApplyToDomestic,EmployerContributionAmount,EmployerContributionCurrency,DomesticHelpComment,PurchaseUsingForeignCurrency,CurrencyName,ListCurrencies,TaxAddedAtPurchase,Commodity1,TaxCommodity1,Commodity2,TaxCommodity2,Commodity3,TaxCommodity3,Commodity4,TaxCommodity4,Commodity5,TaxCommodity5,Commodity6,TaxCommodity6,Commodity7,TaxCommodity7,Commodity8,TaxCommodity8,Commodity9,TaxCommodity9,Commodity10,TaxCommodity10,SurveyMonth,SurveyYear,SurveyType")] ScReport scReport)
        {
            if (syscode != scReport.Syscode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScReportExists(scReport.Syscode))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Create));
            }
            return View(scReport);
        }

        // GET: ScReports/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scReport = await _context.ScReport
                .FirstOrDefaultAsync(m => m.Syscode == id);
            if (scReport == null)
            {
                return NotFound();
            }

            return View(scReport);
        }

        // POST: ScReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var scReport = await _context.ScReport.FindAsync(id);
            _context.ScReport.Remove(scReport);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScReportExists(string id)
        {
            return _context.ScReport.Any(e => e.Syscode == id);
        }
    }
}
