using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities
{
    public class Employee : Entity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Department Department { get; set; }
    }
}
