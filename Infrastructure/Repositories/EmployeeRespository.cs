using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public abstract class EmployeeRespository : IEmployeeRepository
    {
        public abstract Task<Employee> Add(Employee entity);

        public abstract Task<bool> Delete(int id);

        public abstract Task<IEnumerable<Employee>> GetAll();

        public abstract Task<Employee> GetById(int id);

        public abstract Task<Employee> Update(Employee entity);
    }
}
