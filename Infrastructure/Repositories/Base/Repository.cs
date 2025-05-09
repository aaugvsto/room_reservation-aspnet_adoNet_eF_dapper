using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Base;
using Domain.Interfaces.Repositories.Base;

namespace Infrastructure.Repositories.Base
{
    public abstract class Repository<T> : IRepository<T> where T : Entity
    {
        public abstract Task<T> Add(T entity);
        public abstract Task<bool> Delete(int id);
        public abstract Task<IEnumerable<T>> GetAll();
        public abstract Task<T> GetById(int id);
        public abstract Task<T> Update(T entity);
    }
}
