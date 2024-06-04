﻿
using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        Task<(IEnumerable<EmployeeDto> employees, MetaData metadata)> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
        Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
        Task<EmployeeDto> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges);
        Task DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges);
        Task UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges);
        Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyid, Guid id, bool compTrackChanges, bool empTrackChanges);
        Task SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);
    }
}
