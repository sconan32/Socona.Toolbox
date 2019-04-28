using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Socona.ToolBox.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetAllImplementations(this Type baseType, bool everything = false, Assembly[] assemblies = null)
        {
            if (assemblies == null)
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }

            var types = assemblies
                .SelectMany(s => s.GetTypes())
                .Where(p => baseType.IsAssignableFrom(p) && (everything || !(p.IsAbstract || p.IsNestedPrivate || p.IsInterface)));
            return types;
        }
    }
}
