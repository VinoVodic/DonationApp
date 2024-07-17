using CharityPlatform.Domain.Identity;
using CharityPlatform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharityPlatform.Repository.Implementation
{
    public class DonatorRepository : IDonatorRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Donator> entities;
        string errorMessage = string.Empty;

        public DonatorRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Donator>();
        }

        public IEnumerable<Donator> GetAll()
        {
            return entities.AsEnumerable();
        }

        public Donator Get(string id)
        {
            var strGuid = id.ToString();
            return entities
                .Include(z => z.Donations)
                .First(s => s.Id == strGuid);
        }
        public void Insert(Donator entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(Donator entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(Donator entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}
