using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class EmployeeRespositoryEF : EmployeeRespository
    {
        public override Task<Employee> Add(Employee entity)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<Employee>> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Task<Employee> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override Task<Employee> Update(Employee entity)
        {
            throw new NotImplementedException();
        }
    }
}
