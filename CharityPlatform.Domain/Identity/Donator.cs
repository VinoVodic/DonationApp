using CharityPlatform.Domain.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharityPlatform.Domain.Identity
{
    public class Donator : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Email { get; set; }
        //public string PhoneNumber { get; set; }
        public virtual ICollection<Donation>? Donations { get; set; }

        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}
