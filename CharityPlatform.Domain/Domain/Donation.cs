using CharityPlatform.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CharityPlatform.Domain.Domain
{
    public class Donation : BaseEntity
    {
        public double Amount { get; set; }
        public DateOnly DonationDate { get; set; }
        public Guid CampaignId { get; set; }
        public Campaign? Campaign { get; set; }
        public string? DonatorId { get; set; }
        public Donator? Donator { get; set; }
    }
}
