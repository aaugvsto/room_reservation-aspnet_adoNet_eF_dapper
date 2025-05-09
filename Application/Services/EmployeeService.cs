using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Application.Services
{

    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public Task<Employee> AddAsync(Employee entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await this._employeeRepository.GetAll();
        }

        public Task<Employee> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> UpdateAsync(Employee entity)
        {
            throw new NotImplementedException();
        }
    }
}
