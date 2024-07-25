﻿using CharityPlatform.Domain.Domain;
using CharityPlatform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace CharityPlatform.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext context;
        private DbSet<T> entities;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            if (typeof(T).IsAssignableFrom(typeof(Campaign)))
            {
                return entities
                    .Include("CharityOrganization")
                    .Include("Donations")
                    .AsEnumerable();
            }
            else if (typeof(T).IsAssignableFrom(typeof(Donation)))
            {
                return entities
                    .Include("Donator")
                    .Include("Campaign")
                    .Include("Campaign.CharityOrganization")
                    .AsEnumerable();
            }
            else
            {
                return entities.AsEnumerable();
            }
        }

        public T Get(Guid? id)
        {
            if (typeof(T).IsAssignableFrom(typeof(CharityOrganization)))
            {
                return entities
                    .Include("Campaigns")
                    .First(s => s.Id == id);
            }
            else if (typeof(T).IsAssignableFrom(typeof(Campaign)))
            {
                return entities
                    .Include("CharityOrganization")
                    .Include("Donations")
                    .Include("Donations.Donator")
                    .First(s => s.Id == id);
            }
            else if (typeof(T).IsAssignableFrom(typeof(Donation)))
            {
                return entities
                    .Include("Donator")
                    .Include("Campaign")
                    .Include("Campaign.CharityOrganization")
                    .First(s => s.Id == id);
            }
            else
            {
                return entities.First(s => s.Id == id);
            }
        }
        public T Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
            return entity;
        }

        public T Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
            return entity;
        }

        public T Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
            return entity;
        }

        public List<T> InsertMany(List<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }
            entities.AddRange(entities);
            context.SaveChanges();
            return entities;
        }
    }
}
