﻿using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects;

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

        public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
        {
            var company = _repositoryManager.Company.GetCompany(companyId, false);

            if (company == null) throw new CompanyNotFoundException(companyId);

            var employees = _repositoryManager.Employee.GetEmployees(companyId, trackChanges);
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public EmployeeDto GetEmployee(Guid companyId, Guid employeeId,  bool trackChanges)
        {
            var company = _repositoryManager.Company.GetCompany(companyId, false);

            if (company == null)
                throw new CompanyNotFoundException(companyId);

            var employee = _repositoryManager.Employee.GetEmployee(companyId, employeeId, trackChanges);

            if (employee == null)
                throw new EmployeeNotFoundException(employeeId);

            return _mapper.Map<EmployeeDto>(employee);
        }
    }
}
