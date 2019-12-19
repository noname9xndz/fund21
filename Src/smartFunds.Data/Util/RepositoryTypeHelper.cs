using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace smartFunds.Data.Util
{
    public static class RepositoryTypeHelper
    {
        public static List<Type> GetRepositoryTypes()
        {
            var namespaces = new List<string> { "smartFunds.Data.Repositories" };

            List<Type> types = new List<Type>();
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var ns in namespaces)
            {
                var repositoryTypes = assembly.GetTypes().Where(t => string.Equals(t.Namespace, ns, StringComparison.Ordinal)).ToList();

                foreach (var type in repositoryTypes)
                {
                    if (type.GetCustomAttribute(typeof(CompilerGeneratedAttribute), true) == null)
                    {
                        types.Add(type);
                    }
                }
            }

            types.RemoveAll(x => x.Name.ToLower().StartsWith("i"));
            return types;
        }
    }
}
