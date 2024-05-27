using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId:guid}/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public EmployeesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public ActionResult GetEmployees(Guid companyId)
        {
            var employees = _serviceManager.EmployeeService.GetEmployees(companyId, false);
            return Ok(employees);
        }

        [HttpGet("{employeeId}")]
        public ActionResult GetEmployee(Guid companyId, Guid employeeId)
        {
            var employee = _serviceManager.EmployeeService.GetEmployee(companyId, employeeId, false);
            return Ok(employee);
        }
    }
}
