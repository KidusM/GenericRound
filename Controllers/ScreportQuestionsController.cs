using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CSMS.Data;
using CSMS.Models;

using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace CSMS.Controllers
{
    [Authorize]
    public class ScreportQuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScreportQuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ScreportQuestions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ScreportQuestions.Include(s => s.Survey);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ScreportQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screportQuestion = await _context.ScreportQuestions
                .Include(s => s.Survey)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (screportQuestion == null)
            {
                return NotFound();
            }

            return View(screportQuestion);
        }

        // GET: ScreportQuestions/Create
        public IActionResult Create()
        {
            //var loggedInUserId = HttpContext.Session.GetString("_loggedInUserId");
            
            var loggedInUserId = HttpContext.Session.GetString("_loggedInUserId");
            ViewData["ui"] = loggedInUserId;

            //var itemm = _context.Surveys.Where(x => x.Scid == loggedInUserId);
            //var loggedInUserDsid = itemm.Select(xx => xx.Id).First().ToString();
            //HttpContext.Session.SetString("_loggedInUserDSId", loggedInUserDsid);
           // var loggedInUserDsid = HttpContext.Session.GetString("_loggedInUserDSId");
            if (AllowQuestionnaireCreation(loggedInUserId)) {
                //ViewData["SurveyId"] = new SelectList(_context.Surveys, "Id", "Dsid");
                return View();
            }
            else
            {
                return View("Denied");
            }
        }

        // POST: ScreportQuestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SurveyId,CommercialOutSide,CommercialInSide,GovtOutSide,GovtInSide,UnoutSide,UninSide,OtherHousingOutside,OtherHousingInside,OtherHousing,OverallQuality,OverallQualityComments,Availability,AvailabilityComments,MaintenanceLevel,MaintenanceComments,TimeForNewComer,TimeForNewComerComments,KeyMoneySecurityDeposit,AdditionalSecurityCost,SecurityCostReimbursed,FullyReimbursedAllAgencies,FullyReimbursedSomeAgencies,PartiallyReimbursedAllAgencies,PartiallyReimbursedSomeAgencies,AgenciesProvidingReimbursement,AlarmInstallationFrequency,AlarmInstallationPercentage,AlarmInstallationMaxAmount,AlarmInstallationCurrency,AlarmInstallationEffectiveDate,AlarmInstallationComments,HiringAlarmSystemFrequency,HiringAlarmSystemPercentage,HiringAlarmSystemMaxAmount,HiringAlarmSystemCurrency,HiringAlarmSystemEffectiveDate,HiringAlarmSystemComments,BarsFrequency,BarsPercentage,BarsMaxAmount,BarsCurrency,BarsEffectiveDate,BarsComments,GuardsFrequency,GuardsPercentage,GuardsMaxAmount,GuardsCurrency,GuardsEffectiveDate,GuardsComments,Other1Frequency,Other1Percentage,Other1MaxAmount,Other1Currency,Other1EffectiveDate,Other1Comments,Other2Frequency,Other2Percentage,Other2MaxAmount,Other2Currency,Other2EffectiveDate,Other2Comments,CommonToEmployDomestic,SocialInsuranceExists,SocialInsuranceApplyToDomestic,EmployerContributionAmount,EmployerContributionCurrency,DomesticHelpComment,PurchaseUsingForeignCurrency,CurrencyName,ListCurrencies,TaxAddedAtPurchase,Commodity1,TaxCommodity1,Commodity2,TaxCommodity2,Commodity3,TaxCommodity3,Commodity4,TaxCommodity4,Commodity5,TaxCommodity5,Commodity6,TaxCommodity6,Commodity7,TaxCommodity7,Commodity8,TaxCommodity8,Commodity9,TaxCommodity9,Commodity10,TaxCommodity10,SurveyMonth,SurveyYear,SurveyType")] ScreportQuestion screportQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(screportQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SurveyId"] = new SelectList(_context.Surveys, "Id", "Dsid", screportQuestion.SurveyId);
            return View(screportQuestion);
        }

        // GET: ScreportQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screportQuestion = await _context.ScreportQuestions.FindAsync(id);
            if (screportQuestion == null)
            {
                return NotFound();
            }
            ViewData["SurveyId"] = new SelectList(_context.Surveys, "Id", "Dsid", screportQuestion.SurveyId);
            return View(screportQuestion);
        }

        // POST: ScreportQuestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SurveyId,CommercialOutSide,CommercialInSide,GovtOutSide,GovtInSide,UnoutSide,UninSide,OtherHousingOutside,OtherHousingInside,OtherHousing,OverallQuality,OverallQualityComments,Availability,AvailabilityComments,MaintenanceLevel,MaintenanceComments,TimeForNewComer,TimeForNewComerComments,KeyMoneySecurityDeposit,AdditionalSecurityCost,SecurityCostReimbursed,FullyReimbursedAllAgencies,FullyReimbursedSomeAgencies,PartiallyReimbursedAllAgencies,PartiallyReimbursedSomeAgencies,AgenciesProvidingReimbursement,AlarmInstallationFrequency,AlarmInstallationPercentage,AlarmInstallationMaxAmount,AlarmInstallationCurrency,AlarmInstallationEffectiveDate,AlarmInstallationComments,HiringAlarmSystemFrequency,HiringAlarmSystemPercentage,HiringAlarmSystemMaxAmount,HiringAlarmSystemCurrency,HiringAlarmSystemEffectiveDate,HiringAlarmSystemComments,BarsFrequency,BarsPercentage,BarsMaxAmount,BarsCurrency,BarsEffectiveDate,BarsComments,GuardsFrequency,GuardsPercentage,GuardsMaxAmount,GuardsCurrency,GuardsEffectiveDate,GuardsComments,Other1Frequency,Other1Percentage,Other1MaxAmount,Other1Currency,Other1EffectiveDate,Other1Comments,Other2Frequency,Other2Percentage,Other2MaxAmount,Other2Currency,Other2EffectiveDate,Other2Comments,CommonToEmployDomestic,SocialInsuranceExists,SocialInsuranceApplyToDomestic,EmployerContributionAmount,EmployerContributionCurrency,DomesticHelpComment,PurchaseUsingForeignCurrency,CurrencyName,ListCurrencies,TaxAddedAtPurchase,Commodity1,TaxCommodity1,Commodity2,TaxCommodity2,Commodity3,TaxCommodity3,Commodity4,TaxCommodity4,Commodity5,TaxCommodity5,Commodity6,TaxCommodity6,Commodity7,TaxCommodity7,Commodity8,TaxCommodity8,Commodity9,TaxCommodity9,Commodity10,TaxCommodity10,SurveyMonth,SurveyYear,SurveyType")] ScreportQuestion screportQuestion)
        {
            if (id != screportQuestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(screportQuestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScreportQuestionExists(screportQuestion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SurveyId"] = new SelectList(_context.Surveys, "Id", "Dsid", screportQuestion.SurveyId);
            return View(screportQuestion);
        }

        // GET: ScreportQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screportQuestion = await _context.ScreportQuestions
                .Include(s => s.Survey)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (screportQuestion == null)
            {
                return NotFound();
            }

            return View(screportQuestion);
        }

        // POST: ScreportQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var screportQuestion = await _context.ScreportQuestions.FindAsync(id);
            _context.ScreportQuestions.Remove(screportQuestion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScreportQuestionExists(int id)
        {
            return _context.ScreportQuestions.Any(e => e.Id == id);
        }

        private bool AllowQuestionnaireCreation(string loggedInUserId)
        {
            //Check if user is a survey coordinator assigned to a survey
            if (_context.Surveys.Any(e => e.Scid == loggedInUserId))
            {
                //Check if Survey for the duty station is still active 
                var activeSurvey = _context.Surveys 
                       .Where(s => s.Scid == loggedInUserId && s.SurveyBegin.Value <= DateTime.Now && s.SurveyEnd.Value >= DateTime.Now)
                       .FirstOrDefault();

                if (activeSurvey != null)
                {
                    var surveyIDnumber = activeSurvey.Id;
                    var test = activeSurvey.SurveyBegin;
                    ViewData["QuestionnaireSurveyId"] = surveyIDnumber;
                //var test = activeSurvey.ToQueryString();

                
                
                
                    //var loggedUserSurveyId = activeSurvey.Id;
                    //HttpContext.Session.SetString("_loggedInUserSurveyId", loggedUserSurveyId.ToString() );

                    // check if survey is completed

                    var questionnaireFilled = _context.ScreportQuestions
                        .Where(s => s.SurveyId == surveyIDnumber)
                        .FirstOrDefault();

                    if(questionnaireFilled is null)
                    {
                        return true;
                    }
                    else
                    {
                        ViewData["QuestionnaireMessage"] = "Questionnaire is complete and can not be modified";
                        return false;
                    }

                    
                }
                else
                {
                    ViewData["QuestionnaireMessage"] = "Survey for the duty station is not active";
                    return false;
                }

                    // else return false
            }
            else
            {
                ViewData["QuestionnaireMessage"] = "User not authorised to access this questionnaire";
                return false;
            }
        }

    }
}
