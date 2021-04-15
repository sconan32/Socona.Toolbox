using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace Socona.ToolBox.Parametrization.Parameters
{

    public class LongParameter : ValueTypeParameter<long>
    {
        ///<summary>
        /// Constructs a long parameter with the given optionID, parameter constraint
        /// and default value.
        /// 
        ///</summary>
        ///<param name="optionID">the unique UserParameter for this parameter</param>
        ///<param name="constraints">the parameter constraints for this long parameter</param>
        ///<param name="defaultValue">the default value</param>
        public LongParameter(OptionAttribute optionID, bool isRequired = false, long? defaultValue = null, long[] candidates = null, params ValidationAttribute[] constraints) :
            base(optionID, isRequired, defaultValue ?? 0, candidates, constraints)
        {
            hasDefaultValue = (defaultValue != null);
        }

        protected override bool TryParse(object obj, out long value)
        {
            if (obj is long)
            {
                value = (long)obj;
                return true;
            }
            if (obj is int)
            {
                value = (long)obj;
                return true;
            }
            if (long.TryParse(obj.ToString(), out value))
            {
                return true;
            }
            value = default;
            return false;
        }

        public override string PlaceHolder => "<64-bit integer>";
    }
}
