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
    public class DonationService : IDonationService
    {
        private readonly IRepository<Donation> _donationRepository;

        public DonationService(IRepository<Donation> donationRepository)
        {
            _donationRepository = donationRepository;
        }

        public void CreateNewDonation(Donation d)
        {
            _donationRepository.Insert(d);
        }

        public void DeleteDonation(Guid? id)
        {
            Donation donation = _donationRepository.Get(id);
            _donationRepository.Delete(donation);
        }

        public List<Donation> GetAllDonations()
        {
            return _donationRepository.GetAll().ToList();
        }

        public Donation GetDetailsForDonation(Guid? id)
        {
            return _donationRepository.Get(id);
        }

        public void UpdateExistingDonation(Donation d)
        {
            _donationRepository.Update(d);
        }
    }
}
