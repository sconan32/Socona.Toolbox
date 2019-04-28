using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Socona.ToolBox.Parametrization.Parameters
{
    public class DateTimeParameter : ValueTypeParameter<DateTime>
    {

        public DateTimeParameter(OptionAttribute optionID, bool isRequired = false, DateTime? defaultValue = null, DateTime[] candidates = null, params ValidationAttribute[] constraints)
            : base(optionID, isRequired, defaultValue, candidates, constraints)
        { }

        protected override bool TryParse(object obj, out DateTime value)
        {
            if (obj is DateTime valdt)
            {
                value = valdt;
                return true;
            }
            if (DateTime.TryParse(obj.ToString(), out value))
            {
                return true;
            }
            value = default;
            return false;
        }
        public static new DateTimeParameter FromPropertyName(string propertyName, Type inType)
        {
            return (DateTimeParameter)ValueTypeParameter<DateTime>.FromPropertyName(propertyName, inType);
        }

    }
}
