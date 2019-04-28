using Socona.ToolBox.Parametrization.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socona.ToolBox.Windows.Parametrization
{
    public static class ParameterExtensions
    {
        public static IBindableParameter ToBindable(this IParameter para)
        {
            var ptype = para.GetType();
            var baseType = ptype.BaseType;
            if (!baseType.IsGenericType)
            {
                throw new InvalidOperationException();
            }
            var vtype = baseType.GetGenericArguments().First();

            var targetType = typeof(BindableParameter<,>);
            var constructedType = targetType.MakeGenericType(ptype, vtype);
            var result = (IBindableParameter)Activator.CreateInstance(constructedType, para);
            return result;
        }
    }
}
