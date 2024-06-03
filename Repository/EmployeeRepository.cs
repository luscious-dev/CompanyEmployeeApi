using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext context) : base(context)
        {

        }

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            Delete(employee);
        }

        public Employee GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
        {
            return FindByCondition(x => x.CompanyId.Equals(companyId) && x.Id.Equals(employeeId), trackChanges).SingleOrDefault();
        }

        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        {
            var employees = FindByCondition(x => x.CompanyId.Equals(companyId), trackChanges)
                .OrderBy(x => x.Name)
                .Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
                .Take(employeeParameters.PageSize)
                .ToList();

            var employeesCount = await FindByCondition(x => x.CompanyId.Equals(companyId), false).CountAsync();

            return new PagedList<Employee>(employees, employeesCount, employeeParameters.PageNumber, employeeParameters.PageSize);
        }
    }
}
