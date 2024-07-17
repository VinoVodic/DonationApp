using CharityPlatform.Domain.Domain;
using CharityPlatform.Service.Implementation;
using CharityPlatform.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CharityPlatform.Web.Controllers
{
    public class CampaignsController : Controller
    {
        private readonly ICampaignService _campaignService;
        private readonly ICharityOrganizationService _charityOrganizationService;

        public CampaignsController(ICampaignService campaignService, ICharityOrganizationService charityOrganizationService)
        {
            _campaignService = campaignService;
            _charityOrganizationService = charityOrganizationService;
        }

        public IActionResult Index()
        {
            Console.WriteLine(_campaignService.GetAllCampaigns().First().Donations.Select(x => x.Amount).Sum());
            return View(_campaignService.GetAllCampaigns());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campaign = _campaignService.GetDetailsForCampaign(id);
            if (campaign == null)
            {
                return NotFound();
            }

            return View(campaign);
        }

        public IActionResult Create(Guid id)
        {
            CharityOrganization charityOrganization = _charityOrganizationService.GetDetailsForCharityOrganization(id);
            Campaign campaign = new Campaign
            {
                CharityOrganizationId = id,
                CharityOrganization = charityOrganization
            };
            return View(campaign);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,Description,StartDate,EndDate,CharityOrganizationId,CharityOrganization")] Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                campaign.Id = Guid.NewGuid();
                campaign.MoneyRaised = 0.0;
                campaign.Donations = new List<Donation>();
                _campaignService.CreateNewCampaign(campaign);
                return RedirectToAction(nameof(Index));
            }
            return View(campaign);
        }

        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campaign = _campaignService.GetDetailsForCampaign(id);
            if (campaign == null)
            {
                return NotFound();
            }
            return View(campaign);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Title,Description,StartDate,EndDate,CharityOrganizationId,CharityOrganization,MoneyRaised,Donations")] Campaign campaign)
        {
            if (id != campaign.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _campaignService.UpdateExistingCampaign(campaign);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(campaign);
        }

        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campaign = _campaignService.GetDetailsForCampaign(id);
            if (campaign == null)
            {
                return NotFound();
            }

            return View(campaign);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _campaignService.DeleteCampaign(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
