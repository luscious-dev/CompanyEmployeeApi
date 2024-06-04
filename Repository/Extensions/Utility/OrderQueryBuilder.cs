using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions.Utility
{
    // Initially implemented in the RepositoryEmployeeExtension class but had to move
    // it here because something similar will be used for Company sorting. let's avoid repeating ourselves 
    public static class OrderQueryBuilder
    {
        public static string CreateQueryBuilder<T>(string orderByQueryString) where T : class
        {
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var orderQueryBuilder = new StringBuilder();
            
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyNameFromQuery = param.Split(' ')[0];
                var objectProperty = propertyInfos.FirstOrDefault(x => x.Name.Equals(propertyNameFromQuery, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty is null)
                    continue;

                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
            }

            return orderQueryBuilder.ToString().TrimEnd(',', ' ');
        }
    }
}
