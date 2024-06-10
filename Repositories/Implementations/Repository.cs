using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : BaseIdentityEntity
    {
        protected DbSet<T> DbSet { get; set; }

        protected DbContext Context { get; set; }

        public Repository(DbContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context), "An instance of DbContext is required to use this repository.");
            this.DbSet = this.Context.Set<T>();
        }

        //public void ActivateDeactivate(T entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public void ActivateDeactivate(int id)
        //{
        //    throw new NotImplementedException();
        //}

        public void Delete(T entity)
        {
            EntityEntry<T> entry = this.Context.Entry(entity);
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                this.DbSet.Attach(entity);
                this.DbSet.Remove(entity);
            }
        }

        public void Delete(int id)
        {
            var entity = this.GetByIdAsync(id).Result;
            if (entity != null)
            {
                this.Delete(entity);
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync() => await this.DbSet.AsQueryable().ToListAsync();

        //protected static IQueryable<T> SoftDeleteQueryFilter(IQueryable<T> query, bool? isActive)
        //{
        //    if (isActive.HasValue)
        //    {
        //        query = query.Where(x => x.IsActivated == isActive.Value);
        //    }
        //    return query;
        //}

        public async Task<T> GetByIdAsync(int id) => await this.DbSet.FindAsync(id);

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await this.DbSet.AnyAsync(predicate);
        }

        public void Insert(T entity)
        {
            EntityEntry<T> entry = this.Context.Entry(entity);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                this.DbSet.AddAsync(entity);
            }
        }

        public void Update(T entity, string excludeProperties = "")
        {
            EntityEntry<T> entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;

            foreach (var excludeProperty in excludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                entry.Property(excludeProperty).IsModified = false;
            }

        }

        public virtual void Save(T entity)
        {
            if (entity.Id == 0)
            {
                this.Insert(entity);
            }
            else
            {
                this.Update(entity);
            }
        }
    }
}
