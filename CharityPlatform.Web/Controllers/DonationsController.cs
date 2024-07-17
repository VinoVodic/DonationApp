using CharityPlatform.Domain.Domain;
using CharityPlatform.Repository.Interface;
using CharityPlatform.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CharityPlatform.Web.Controllers
{
    public class DonationsController : Controller
    {
        private readonly IDonationService _donationService;
        private readonly ICampaignService _campaignService;
        private readonly IDonatorRepository _donatorRepository;

        public DonationsController(IDonationService donationService, ICampaignService campaignService, IDonatorRepository donatorRepository)
        {
            _donationService = donationService;
            _campaignService = campaignService;
            _donatorRepository = donatorRepository;
        }

        public IActionResult Index()
        {
            return View(_donationService.GetAllDonations());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = _donationService.GetDetailsForDonation(id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        public IActionResult Create(Guid id)
        {
            Campaign campaign = _campaignService.GetDetailsForCampaign(id);
            Donation donation = new Donation
            {
                CampaignId = id,
                Campaign = campaign
            };
            return View(donation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Amount,CampaignId,Campaign")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                donation.Id = Guid.NewGuid();
                donation.DonationDate = DateOnly.FromDateTime(DateTime.Now);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var loggedInUser = _donatorRepository.Get(userId);
                donation.DonatorId = userId;
                donation.Donator = loggedInUser;
                _donationService.CreateNewDonation(donation);  
                return RedirectToAction(nameof(Index));
            }
            return View(donation);
        }

        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = _donationService.GetDetailsForDonation(id);
            if (donation == null)
            {
                return NotFound();
            }
            return View(donation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //TODO dali treba donator i donatorID
        public IActionResult Edit(Guid id, [Bind("Id,Amount,CampaignId,Campaign,DonatorId,Donator")] Donation donation)
        {
            if (id != donation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _donationService.UpdateExistingDonation(donation);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(donation);
        }

        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = _donationService.GetDetailsForDonation(id);
            if (donation == null)
            {
                return NotFound();
            }
            return View(donation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _donationService.DeleteDonation(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
