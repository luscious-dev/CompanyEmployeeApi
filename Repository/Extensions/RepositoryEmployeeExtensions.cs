using Entities.Models;
using Repository.Extensions.Utility;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Repository.Extensions
{
    public static class RepositoryEmployeeExtensions
    {
        public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees, uint minAge, uint maxAge)
        {
            return employees.Where(x => x.Age >= minAge && x.Age <= maxAge);
        }

        public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string searchTerm)
        {
            
            if (string.IsNullOrWhiteSpace(searchTerm))
                return employees;
            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return employees.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string orderByQueryString)
        {
            if(string.IsNullOrWhiteSpace(orderByQueryString))
                return employees.OrderBy(x => x.Name);

            // Created the OrderQueryBuilder.CreateQueryBuilder because we would like to reuse the code for Company sorting too
            var orderQuery = OrderQueryBuilder.CreateQueryBuilder<Employee>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return employees.OrderBy(e => e.Name);

            return employees.OrderBy(orderQuery);

        }

        public static IQueryable<Employee> Limit(this IQueryable<Employee> employees, string limitQueryString)
        {
            if (string.IsNullOrEmpty(limitQueryString))
                return employees;
            
            var fields = limitQueryString.Trim().Split(',');
            var propertyInfo = typeof(Employee).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach(var field in fields)
            {
                if(string.IsNullOrWhiteSpace(field))
                    continue;
                var objectProperty = propertyInfo.FirstOrDefault(x => x.Name.Equals(field, StringComparison.InvariantCultureIgnoreCase));

                if(objectProperty == null)
                    continue;

            }
            return employees;
        }
    }
}
