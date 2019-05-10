using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Socona.ToolBox.Parametrization.Parameters
{
    public class IntParameter : ValueTypeParameter<int>
    {
        ///<summary>
        /// Constructs an integer parameter with the given optionID, parameter
        /// constraint, and default value.
        /// 
        ///   </summary>
        ///<param name="optionID">optionID the unique id of the option</param>
        ///<param name="constraints">the constraint for this integer parameter</param>
        ///<param name="defaultValue">the default value</param>
        public IntParameter(OptionAttribute optionID, bool isRequired = false, int? defaultValue = null, IEnumerable<int> candidates = null, IEnumerable<ValidationAttribute> constraints = null) :
            base(optionID, isRequired, defaultValue ?? 0, candidates, constraints)
        {
            _hasDefaultValue = (defaultValue != null);
        }

        protected override bool TryParse(object obj, out int value)
        {
            if (obj is int)
            {
                value = (int)obj;
                return true;
            }
            if (int.TryParse(obj.ToString(), out value))
            {
                return true;
            }
            value = default;
            return false;
        }

        public override string PlaceHolder => "<integer>";
    }
}