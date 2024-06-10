using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IRepository<T> where T : BaseIdentityEntity
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        void Insert(T entity);

        void Update(T entity, string excludeProperties = "");

        //void ActivateDeactivate(T entity);

        //void ActivateDeactivate(int id);

        void Delete(T entity);

        void Delete(int id);
    }
}
