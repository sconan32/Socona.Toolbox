using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Socona.ToolBox.Parametrization.Parameters
{
    public class DoubleParameter : ValueTypeParameter<double>
    {
        ///<summary>
        /// Constructs a double parameter with the given optionID, parameter
        /// constraints, and default value.
        /// 
        ///   </summary>
        ///<param name="optionID">the unique optionID</param>
        ///<param name="cons">a list of parameter constraints for this double parameter</param>
        ///<param name="defaultValue">the default value for this double parameter</param>
        public DoubleParameter(OptionAttribute optionID, bool isRequired = false, double? defaultValue = null, double[] candidates = null, params ValidationAttribute[] constraints)
      : base(optionID, isRequired, defaultValue ?? 0, candidates, constraints)
        { }

        protected override bool TryParse(object obj, out double value)
        {
            if (obj is double vald)
            {
                value = vald;
                return true;
            }
            if (double.TryParse(obj.ToString(), out value))
            {
                return true;
            }
            value = default;
            return false;
        }

        public override string PlaceHolder => "<number>";
    }
}
