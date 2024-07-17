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
    public class CharityOrganizationService : ICharityOrganizationService
    {
        private readonly IRepository<CharityOrganization> _charityOrganizationRepository;

        public CharityOrganizationService(IRepository<CharityOrganization> charityOrganizationRepository)
        {
            _charityOrganizationRepository = charityOrganizationRepository;
        }

        public void CreateNewCharityOrganization(CharityOrganization co)
        {
           _charityOrganizationRepository.Insert(co);
        }

        public void DeleteCharityOrganization(Guid? id)
        {
            CharityOrganization co = _charityOrganizationRepository.Get(id);
            _charityOrganizationRepository.Delete(co);
        }

        public List<CharityOrganization> GetAllCharityOrganizations()
        {
            return _charityOrganizationRepository.GetAll().ToList();
        }

        public CharityOrganization GetDetailsForCharityOrganization(Guid? id)
        {
            return _charityOrganizationRepository.Get(id);
        }

        public void UpdateExistingCharityOrganization(CharityOrganization co)
        {
            _charityOrganizationRepository.Update(co);
        }
    }
}
