using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Base;
using Domain.Interfaces.Repositories.Base;
using Domain.Interfaces.Services.Base;

namespace Application.Services.Base
{
    public abstract class Service<T> : IService<T> where T : Entity
    {
        private readonly IRepository<T> _repository;

        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<T> AddAsync(T entity)
        {
            return await _repository.Add(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            return await _repository.Update(entity);
        }
    }
}
