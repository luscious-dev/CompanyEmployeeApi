﻿using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    internal sealed class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext context) : base(context)
        {

        }

        public void CreateCompany(Company company)
        {
            Create(company);
            return;
        }

        public void DeleteCompany(Company company)
        {
            Delete(company);
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            return await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();
        }

        public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges)
        {
            return await FindByCondition(x => x.Id.Equals(companyId), trackChanges).SingleOrDefaultAsync();
        }

    }
}
