using CharityPlatform.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharityPlatform.Repository.Interface
{
    public interface IDonatorRepository
    {
        IEnumerable<Donator> GetAll();
        Donator Get(string id);
        void Insert(Donator entity);
        void Update(Donator entity);
        void Delete(Donator entity);
    }
}
