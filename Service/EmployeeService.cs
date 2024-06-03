using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<EmployeeDto> employees, MetaData metadata)> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        {
            var company = await _repositoryManager.Company.GetCompanyAsync(companyId, false);

            if (company == null) throw new CompanyNotFoundException(companyId);

            if (!employeeParameters.ValidAgeRange)
                throw new MaxAgeRangeBadRequestException();

            var employeesWithMetaData = await _repositoryManager.Employee.GetEmployeesAsync(companyId, employeeParameters,  trackChanges);
            var employeesDto =  _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);
            return (employeesDto, employeesWithMetaData.MetaData);
        }

        public EmployeeDto GetEmployee(Guid companyId, Guid employeeId,  bool trackChanges)
        {
            var company = _repositoryManager.Company.GetCompanyAsync(companyId, false);

            if (company == null)
                throw new CompanyNotFoundException(companyId);

            var employee = _repositoryManager.Employee.GetEmployee(companyId, employeeId, trackChanges);

            if (employee == null)
                throw new EmployeeNotFoundException(employeeId);

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
        {
            var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);
            if(company == null)
                throw new CompanyNotFoundException(companyId);

            var employeeEntity = _mapper.Map<Employee>(employeeForCreation);

            _repositoryManager.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            await _repositoryManager.SaveAsync();

            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

            return employeeToReturn;
        }

        public async Task DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
        {
            var company = await _repositoryManager.Company.GetCompanyAsync(companyId, false);
            if(company == null)
                throw new CompanyNotFoundException(companyId);

            var employeeForCompany = _repositoryManager.Employee.GetEmployee(companyId, id, trackChanges);
            if(employeeForCompany == null)
                throw new EmployeeNotFoundException(id);

            _repositoryManager.Employee.DeleteEmployee(employeeForCompany);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
        {
            var company = await _repositoryManager.Company.GetCompanyAsync(companyId, empTrackChanges);
            if(company is null)
                throw new CompanyNotFoundException(companyId);

            var employeeEntity = _repositoryManager.Employee.GetEmployee(companyId, id, empTrackChanges);
            if (employeeEntity is null)
                throw new EmployeeNotFoundException(id);

            _mapper.Map(employeeForUpdate, employeeEntity);
            await _repositoryManager.SaveAsync();
        }

        public (EmployeeForUpdateDto employeeToPatch, Employee employeeEntity) GetEmployeeForPatch(Guid companyid, Guid id, bool compTrackChanges, bool empTrackChanges)
        {
            var company = _repositoryManager.Company.GetCompanyAsync(companyid, compTrackChanges);
            if(company is null)
                throw new CompanyNotFoundException(companyid);

            var employeeEntity = _repositoryManager.Employee.GetEmployee(companyid, id, empTrackChanges);
            if(employeeEntity is null)
                throw new EmployeeNotFoundException(id);

            var employeeToPatch  = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
            return (employeeToPatch, employeeEntity);
        }

        public async Task SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity);
            await _repositoryManager.SaveAsync();
        }
    }
}
