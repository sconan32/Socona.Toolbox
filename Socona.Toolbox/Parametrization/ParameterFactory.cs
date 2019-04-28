using Socona.ToolBox.Parametrization.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Socona.ToolBox.Parametrization
{
    public class ParameterFactory
    {

        public static ParameterFactory Instance => new ParameterFactory();

        private ParameterFactory()
        {

        }

        public IntParameter Build(OptionAttribute optionID, bool isRequired = false, int? defaultValue = default, ValidationAttribute[] constraints = null, int[] candidates = null)
        {
            return new IntParameter(optionID, isRequired, defaultValue, candidates, constraints);
        }

        internal bool TryBuildFromType(Type type, out IParameter parameter)
        {
            var attrs = type.GetCustomAttributes(true);
            var oa = attrs.OfType<OptionAttribute>();
            if (oa.Count() == 1)
            {
                if (typeof(ISettings).IsAssignableFrom(type))
                {
                    var spec = FromAttribute(oa.Single(), type, null);
                    if (spec.Name.Length == 0 && spec.FullName.Length == 0)
                    {
                        spec.FullName = type.Name.ToLowerInvariant();
                    }
                    var vas = attrs.OfType<ValidationAttribute>();
                    foreach (var va in vas)
                    {
                        spec.AddConstraint(va);
                    }
                    parameter = spec;
                    return true;
                }
            }

            parameter = null;
            return false;
        }

        //public Parameter<T> Build<T>(OptionAttribute optionID, bool isRequired = false, T defaultValue = default, ValidationAttribute[] constraints=null, T[] candidates=null)
        //{
        //    if(T is bool)
        //    {

        //    }
        //}
        public bool TryBuildFromProperty(PropertyInfo property, out IParameter parameter)
        {
            var attrs = property.GetCustomAttributes(true);
            var oa = attrs.OfType<OptionAttribute>();
            if (oa.Count() == 1)
            {
                var spec = FromAttribute(oa.Single(), property.PropertyType,
                    property.PropertyType.GetTypeInfo().IsEnum
                        ? Enum.GetNames(property.PropertyType)
                        : Enumerable.Empty<string>());
                if (spec.Name.Length == 0 && spec.FullName.Length == 0)
                {
                    spec.FullName = property.Name.ToLowerInvariant();
                }
                var vas = attrs.OfType<ValidationAttribute>();
                foreach (var va in vas)
                {
                    spec.AddConstraint(va);
                }
                parameter = spec;
                return true;
            }
            parameter = null;
            return false;
        }


        public IParameter FromPropertyName(string propertyName, Type inType)
        {
            PropertyInfo property = inType.GetProperty(propertyName);
            var attrs = property.GetCustomAttributes(true);
            var oa = attrs.OfType<OptionAttribute>();
            if (oa.Count() == 1)
            {
                var spec = FromAttribute(oa.Single(), property.PropertyType,
                    property.PropertyType.GetTypeInfo().IsEnum
                        ? Enum.GetNames(property.PropertyType)
                        : Enumerable.Empty<string>());
                if (spec.Name.Length == 0 && spec.FullName.Length == 0)
                {
                    spec.FullName = property.Name.ToLowerInvariant();
                }
                var vas = attrs.OfType<ValidationAttribute>();
                foreach (var va in vas)
                {
                    spec.AddConstraint(va);
                }

                return spec;
            }
            throw new InvalidOperationException();
        }
        public IParameter FromAttribute(OptionAttribute oa, Type type, IEnumerable<object> candidates)
        {
            IParameter result = null;
            if (type == typeof(int))
            {
                result = new IntParameter(oa);

            }
            else if (type == typeof(bool))
            {
                result = new BoolParameter(oa);
            }
            else if (type == typeof(DateTime))
            {
                result = new DateTimeParameter(oa);
            }
            else if (typeof(ISettings).IsAssignableFrom(type))
            {
                result = new TypeParameter(oa, type);
            }

            return result;
        }

    }

}



