using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Text.Json;

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
        public async Task<ActionResult> GetEmployees(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
        {
            var pagedResult = await _serviceManager.EmployeeService.GetEmployeesAsync(companyId, employeeParameters, false);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metadata));
            return Ok(pagedResult.employees);
        }

        [HttpGet("{employeeId}")]
        public async Task<ActionResult> GetEmployee(Guid companyId, Guid employeeId)
        {
            var employee = await _serviceManager.EmployeeService.GetEmployeeAsync(companyId, employeeId, false);
            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee)
        {
            if (employee is null)
                return BadRequest("EmployeeObject DTO is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            var employeeResult = await _serviceManager.EmployeeService.CreateEmployeeForCompany(companyId, employee, false);
            return CreatedAtRoute("GetEmployeeForCompany", new { companyId, employeeId = employeeResult.Id }, employeeResult);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteEmployeeForCompany(Guid companyId,Guid id)
        {
            _serviceManager.EmployeeService.DeleteEmployeeForCompany(companyId, id, false);
            return NoContent();
        }

        [HttpPut("{id}")]

        // Put request is a request for full update. If we set only the Age property
        // in the DTO, the age will be updated but the other properties will be set to
        // their default values
        public ActionResult UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employeeForUpdate)
        {
            if (employeeForUpdate is null)
                return BadRequest("Employee update object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            _serviceManager.EmployeeService.UpdateEmployeeForCompany(companyId, id, employeeForUpdate, false, true);
            return NoContent();
        }

        // Set the content type to application/json-patch+json for it work well
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("PatchDoc object sent from the client is null.");

            var result = await _serviceManager.EmployeeService.GetEmployeeForPatchAsync(companyId, id, false, true);
            
            patchDoc.ApplyTo(result.employeeToPatch, ModelState);

            TryValidateModel(result.employeeToPatch);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            _serviceManager.EmployeeService.SaveChangesForPatch(result.employeeToPatch, result.employeeEntity);
            
            return NoContent();
        }
    }
}
