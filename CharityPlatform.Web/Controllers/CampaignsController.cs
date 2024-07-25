using CharityPlatform.Domain.Domain;
using CharityPlatform.Service.Implementation;
using CharityPlatform.Service.Interface;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace CharityPlatform.Web.Controllers {
    public class CampaignsController : Controller {
        private readonly ICampaignService _campaignService;
        private readonly ICharityOrganizationService _charityOrganizationService;

        public CampaignsController(ICampaignService campaignService, ICharityOrganizationService charityOrganizationService) {
            _campaignService = campaignService;
            _charityOrganizationService = charityOrganizationService;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public IActionResult Index() {
            return View(_campaignService.GetAllCampaigns());
        }

        public async Task<IActionResult> Details(Guid? id) {
            if (id == null) {
                return NotFound();
            }

            var campaign = _campaignService.GetDetailsForCampaign(id);
            if (campaign == null) {
                return NotFound();
            }

            return View(campaign);
        }

        public IActionResult Create(Guid id) {
            CharityOrganization charityOrganization = _charityOrganizationService.GetDetailsForCharityOrganization(id);
            Campaign campaign = new Campaign {
                CharityOrganizationId = id,
                CharityOrganization = charityOrganization
            };
            return View(campaign);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,Description,StartDate,EndDate,CharityOrganizationId,CharityOrganization")] Campaign campaign) {
            if (ModelState.IsValid) {
                campaign.Id = Guid.NewGuid();
                campaign.MoneyRaised = 0.0;
                campaign.Donations = new List<Donation>();
                _campaignService.CreateNewCampaign(campaign);
                return RedirectToAction(nameof(Index));
            }
            return View(campaign);
        }

        public IActionResult Edit(Guid? id) {
            if (id == null) {
                return NotFound();
            }

            var campaign = _campaignService.GetDetailsForCampaign(id);
            if (campaign == null) {
                return NotFound();
            }
            return View(campaign);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Title,Description,StartDate,EndDate,CharityOrganizationId,CharityOrganization,MoneyRaised,Donations")] Campaign campaign) {
            if (id != campaign.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _campaignService.UpdateExistingCampaign(campaign);
                }
                catch (DbUpdateConcurrencyException) {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(campaign);
        }

        public IActionResult Delete(Guid? id) {
            if (id == null) {
                return NotFound();
            }

            var campaign = _campaignService.GetDetailsForCampaign(id);
            if (campaign == null) {
                return NotFound();
            }

            return View(campaign);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id) {
            _campaignService.DeleteCampaign(id);
            return RedirectToAction(nameof(Index));
        }

        public FileContentResult CreateInvoice(Guid id) {

            var campaign = _campaignService.GetDetailsForCampaign(id);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{Title}}", campaign.Title);
            document.Content.Replace("{{Description}}", campaign.Description);
            document.Content.Replace("{{MoneyRaised}}", campaign.FullName.ToString());
            document.Content.Replace("{{StartDate}}", campaign.StartDate.ToString());
            document.Content.Replace("{{EndDate}}", campaign.EndDate.ToString());
            document.Content.Replace("{{Organization}}", campaign.CharityOrganization.Name);


            StringBuilder sb = new StringBuilder();
            foreach (var item in campaign.Donations) {
                sb.AppendLine("Donator: " + item.Donator.FullName + " has donated " + item.Amount);
            }
            document.Content.Replace("{{Donations}}", sb.ToString());

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());
            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }
    }
}
