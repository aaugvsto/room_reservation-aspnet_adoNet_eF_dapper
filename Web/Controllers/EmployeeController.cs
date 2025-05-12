using System.Diagnostics;
using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;
using Web.Models;

namespace Web.Controllers
{
    public class EmployeeController : BaseController<Employee>
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService) : base(employeeService)
        {
            _employeeService = employeeService;
        }
    }
}
