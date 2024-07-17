using CharityPlatform.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharityPlatform.Service.Interface
{
    public interface ICharityOrganizationService
    {
        List<CharityOrganization> GetAllCharityOrganizations();
        CharityOrganization GetDetailsForCharityOrganization(Guid? id);
        void CreateNewCharityOrganization(CharityOrganization co);
        void UpdateExistingCharityOrganization(CharityOrganization co);
        void DeleteCharityOrganization(Guid? id);
    }
}
