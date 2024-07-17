using CharityPlatform.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharityPlatform.Service.Interface
{
    public interface ICampaignService
    {
        List<Campaign> GetAllCampaigns();
        Campaign GetDetailsForCampaign(Guid? id);
        void CreateNewCampaign(Campaign c);
        void UpdateExistingCampaign(Campaign c);
        void DeleteCampaign(Guid? id);
    }
}
