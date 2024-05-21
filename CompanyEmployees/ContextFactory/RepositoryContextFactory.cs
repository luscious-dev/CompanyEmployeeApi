using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace CompanyEmployees.ContextFactory
{
    // Since our database context (RepositoryContext) is in a different project
    // This class will help our app to create a derived DbContext instance during
    // the design time to help us with our migrations
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            // Creating a configuration object and specifying appsettings file
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Console.WriteLine(Directory.GetCurrentDirectory());

            var builder = new DbContextOptionsBuilder<RepositoryContext>()
                .UseSqlServer(configuration.GetConnectionString("SqlConnection"));

            return new RepositoryContext(builder.Options);
        }
    }
}
