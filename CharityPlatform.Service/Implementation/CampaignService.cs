using CharityPlatform.Domain.Domain;
using CharityPlatform.Repository.Interface;
using CharityPlatform.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharityPlatform.Service.Implementation
{
    public class CampaignService : ICampaignService
    {
        private readonly IRepository<Campaign> _campaignRepository;

        public CampaignService(IRepository<Campaign> campaignRepository)
        {
            _campaignRepository = campaignRepository;
        }

        public void CreateNewCampaign(Campaign c)
        {
            _campaignRepository.Insert(c);
        }

        public void DeleteCampaign(Guid? id)
        {
            Campaign campaign = _campaignRepository.Get(id);
            _campaignRepository.Delete(campaign);
        }

        public List<Campaign> GetAllCampaigns()
        {
            return _campaignRepository.GetAll().ToList();
        }

        public Campaign GetDetailsForCampaign(Guid? id)
        {
            return _campaignRepository.Get(id);
        }

        public void UpdateExistingCampaign(Campaign c)
        {
            _campaignRepository.Update(c);  
        }
    }
}
