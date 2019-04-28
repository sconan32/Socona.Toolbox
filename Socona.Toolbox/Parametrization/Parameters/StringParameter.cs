using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Socona.ToolBox.Parametrization.Parameters
{
    public class StringParameter : Parameter<string>
    {
        ///<summary>
        /// Constructs a string parameter with the given optionID, constraints and
        /// default value.       
        ///   </summary>
        ///<param name="optionID">the unique id of the parameter</param>
        ///<param name="constraint">parameter constraint</param>
        ///<param name="defaultValue">the default value of the parameter</param>
        public StringParameter(OptionAttribute optionID, bool isRequired = false,
             string defaultValue = null, string[] candidates = null, params ValidationAttribute[] constraints) :
            base(optionID, isRequired, defaultValue, candidates, constraints)
        { }

        protected override bool TryParse(object obj, out string value)
        {
            if (obj is string valstr)
            {
                value = valstr;
                return true;
            }
            value = null;
            return false;
        }
    }
}
