using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

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

        [HttpGet(Name = "GetEmployeeForCompany")]
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

        [HttpPost]
        public ActionResult CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee)
        {
            var employeeResult = _serviceManager.EmployeeService.CreateEmployeeForCompany(companyId, employee, false);
            return CreatedAtRoute("GetEmployeeForCompany", new { companyId, employeeId = employeeResult.Id }, employeeResult);
        }
    }
}
