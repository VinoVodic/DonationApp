using CharityPlatform.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharityPlatform.Service.Interface
{
    public interface IDonationService
    {
        List<Donation> GetAllDonations();
        Donation GetDetailsForDonation(Guid? id);
        void CreateNewDonation(Donation d);
        void UpdateExistingDonation(Donation d);
        void DeleteDonation(Guid? id);
    }
}
