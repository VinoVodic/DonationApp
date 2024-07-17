using CharityPlatform.Domain.Domain;
using CharityPlatform.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace CharityPlatform.Web.Controllers
{
    public class CharityOrganizationsController : Controller
    {
        private readonly ICharityOrganizationService _charityOrganizationService;
        private readonly ICampaignService _campaignService;

        public CharityOrganizationsController(ICharityOrganizationService charityOrganizationService, ICampaignService campaignService)
        {
            _charityOrganizationService = charityOrganizationService;
            _campaignService = campaignService;
        }

        public IActionResult Index()
        {
            return View(_charityOrganizationService.GetAllCharityOrganizations());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var charityOrganization = _charityOrganizationService.GetDetailsForCharityOrganization(id);
            if (charityOrganization == null)
            {
                return NotFound();
            }

            return View(charityOrganization);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Description,Email,PhoneNumber,Address")] CharityOrganization charityOrganization)
        {
            if (ModelState.IsValid)
            {
                charityOrganization.Id = Guid.NewGuid();
                charityOrganization.Campaigns = new List<Campaign>();
                _charityOrganizationService.CreateNewCharityOrganization(charityOrganization);
                return RedirectToAction(nameof(Index));
            }
            return View(charityOrganization);
        }

        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var charityOrganization = _charityOrganizationService.GetDetailsForCharityOrganization(id);
            if (charityOrganization == null)
            {
                return NotFound();
            }
            return View(charityOrganization);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Name,Description,Email,PhoneNumber,Address")] CharityOrganization charityOrganization)
        {
            if (id != charityOrganization.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _charityOrganizationService.UpdateExistingCharityOrganization(charityOrganization);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(charityOrganization);
        }
        
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var charityOrganization = _charityOrganizationService.GetDetailsForCharityOrganization(id);
            if (charityOrganization == null)
            {
                return NotFound();
            }

            return View(charityOrganization);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _charityOrganizationService.DeleteCharityOrganization(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddCampaign(Guid id)
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
        public IActionResult AddCampaign([Bind("Id,Title,Description,StartDate,EndDate,CharityOrganizationId,CharityOrganization")] Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                campaign.Id = Guid.NewGuid();
                campaign.MoneyRaised = 0.0;
                campaign.Donations = new List<Donation>();
                _campaignService.CreateNewCampaign(campaign);
                return RedirectToAction(nameof(Index));
            }
            return View(_charityOrganizationService.GetAllCharityOrganizations());
        }
    }
}
