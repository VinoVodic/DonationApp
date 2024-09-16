using CharityPlatform.Domain.Domain;
using CharityPlatform.Repository.Interface;
using CharityPlatform.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Security.Claims;

namespace CharityPlatform.Web.Controllers {
    public class DonationsController : Controller {
        private readonly IDonationService _donationService;
        private readonly ICampaignService _campaignService;
        private readonly IDonatorRepository _donatorRepository;

        public DonationsController(IDonationService donationService, ICampaignService campaignService, IDonatorRepository donatorRepository) {
            _donationService = donationService;
            _campaignService = campaignService;
            _donatorRepository = donatorRepository;
        }

        public IActionResult Index() {
            return View(_donationService.GetAllDonations());
        }

        public async Task<IActionResult> Details(Guid? id) {
            if (id == null) {
                return NotFound();
            }

            var donation = _donationService.GetDetailsForDonation(id);
            if (donation == null) {
                return NotFound();
            }

            return View(donation);
        }

        public IActionResult Create(Guid id) {
            Campaign campaign = _campaignService.GetDetailsForCampaign(id);
            Donation donation = new Donation {
                CampaignId = id,
                Campaign = campaign
            };
            return View(donation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Amount,CampaignId,Campaign")] Donation donation) {
            if (ModelState.IsValid) {
                donation.Id = Guid.NewGuid();
                donation.DonationDate = DateOnly.FromDateTime(DateTime.Now);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var loggedInUser = _donatorRepository.Get(userId);
                donation.DonatorId = userId;
                donation.Donator = loggedInUser;
                _donationService.CreateNewDonation(donation);

                return RedirectToAction("Pay", new { id = donation.Id });
            }
            return View(donation);
        }

        public IActionResult Edit(Guid? id) {
            if (id == null) {
                return NotFound();
            }

            var donation = _donationService.GetDetailsForDonation(id);
            if (donation == null) {
                return NotFound();
            }
            return View(donation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Amount,CampaignId,Campaign,DonatorId,Donator")] Donation donation) {
            if (id != donation.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _donationService.UpdateExistingDonation(donation);
                }
                catch (DbUpdateConcurrencyException) {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(donation);
        }

        public IActionResult Delete(Guid? id) {
            if (id == null) {
                return NotFound();
            }

            var donation = _donationService.GetDetailsForDonation(id);
            if (donation == null) {
                return NotFound();
            }
            return View(donation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id) {
            _donationService.DeleteDonation(id);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Pay(Guid id) {
            if (id == null) {
                return NotFound();
            }

            var donation = _donationService.GetDetailsForDonation(id);
            if (donation == null) {
                return NotFound();
            }
            return View(donation);
        }


        public IActionResult PayDonation(string stripeEmail, string stripeToken, Guid donationId) {
            StripeConfiguration.ApiKey = "sk_test_51PgNuB2KZONVMMOmXYZU15JD7VONPYefF5A3GfpI2BCOuRPVZwYGGqJOug81v3xKlJHr3je9Rh6HsVEzNUp5Q51e00zHMzLX7X";

            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (donationId == null) {
                return NotFound();
            }

            var donation = _donationService.GetDetailsForDonation(donationId);
            if (donation == null) {
                return NotFound();
            }

                var customer = customerService.Create(new CustomerCreateOptions {
                    Email = stripeEmail,
                    Source = stripeToken
                });

            var charge = chargeService.Create(new ChargeCreateOptions {
                Amount = (Convert.ToInt32(donation.Amount)),
                Description = "Donation Application Payment",
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded") {
                    return RedirectToAction("SuccessPayment");

            }
            else {
                _donationService.DeleteDonation(donationId);
                return RedirectToAction("UnsuccessPayment");
            }
        }

        public IActionResult SuccessPayment() {
            return View();
        }

        public IActionResult UnsuccessPayment() {
            return View();
        }
    }
}
