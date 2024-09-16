using CharityPlatform.Domain.Domain;
using CharityPlatform.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharityPlatform.Repository
{
    public class ApplicationDbContext : IdentityDbContext<Donator>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public virtual DbSet<CharityOrganization> CharityOrganizations { get; set; }
        public virtual DbSet<Campaign> Campaigns { get; set; }
        public virtual DbSet<Donation> Donations { get; set; }
        public virtual DbSet<Book> Books { get; set; }
    }
}
