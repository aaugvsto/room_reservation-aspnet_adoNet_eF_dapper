using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Base;

namespace Domain.Interfaces.Repositories.Base
{
    public interface IRepository<T> where T : Entity
    {
        public Task<T> GetAll();
        public Task<T> GetById(int id);
        public Task<T> Add(T entity);
        public Task<T> Update(T entity);
        public Task<bool> Delete(int id);
    }
}