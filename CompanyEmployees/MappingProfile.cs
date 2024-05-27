using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace CompanyEmployees
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(c => c.FullAddress, opt => opt.MapFrom(x => string.Join(x.Address, x.Country))); // Valid when dealing with class objects
                //.ForCtorParam("FullAddress", opt => opt.MapFrom(x => string.Join(x.Address, x.Country))); // Valid when dealing with contructors

            CreateMap<Employee, EmployeeDto>();
        }
    }
}
