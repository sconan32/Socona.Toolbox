using Socona.ToolBox.Parametrization;
using Socona.ToolBox.Parametrization.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Socona.Expor.Utilities.Options.Parameters
{

    public class PatternParameter : Parameter<Regex>
    {
        ///<summary>
        /// Constructs a pattern parameter with the given optionID, constraints and
        /// default value.
        /// 
        ///   </summary>
        ///<param name="optionID">the unique id of the parameter</param>
        ///<param name="constraint">parameter constraint</param>
        ///<param name="defaultValue">the default value of the parameter</param>
        public PatternParameter(OptionAttribute optionID, bool isRequired,
            Regex defaultValue, Regex[] candidates, ValidationAttribute[] constraints) :
            base(optionID, isRequired, defaultValue, candidates, constraints)
        { }

        protected override bool TryParse(object obj, out Regex value)
        {
            if (obj is Regex regex)
            {
                value = regex;
                return true;
            }
            if (obj is String str)
            {
                try
                {
                    value = new Regex(str);
                    return true;
                }
                catch (ArgumentException ex)
                {
                    Debug.WriteLine($"Given pattern \"{ obj }\" for parameter \"{ Name }\" is no valid regular expression!\nMessage:{ex.Message}");
                }
            }
            value = null;
            return false;
        }

        public override string PlaceHolder => "<regex>";
    }

}
