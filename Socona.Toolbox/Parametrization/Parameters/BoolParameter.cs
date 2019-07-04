using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socona.ToolBox.Parametrization.Parameters
{
    public class BoolParameter : ValueTypeParameter<bool>
    {
        public BoolParameter(OptionAttribute optionID, bool isRequired = false, bool defaultValue = false)
            : base(optionID, isRequired, defaultValue)
        {
            this.Candidates.Add(true);
            this.Candidates.Add(false);
        }

        protected override bool TryParse(object obj, out bool value)
        {
            if (obj is bool valb)
            {
                value = valb;
                return true;
            }
            if (obj is int vali)
            {
                value = (vali != 0);
                return true;
            }
            if (bool.TryParse(obj.ToString(), out value))
            {
                return true;
            }

            value = false;
            return false;
        }

        ///<summary>
        /// Shorthand for <code>IsValid && IsValid == true</code>
        /// </summary>
        public bool IsTrue => IsValid && IsValid;

        ///<summary>
        /// Shorthand for <code>IsValid && IsValid == false</code>
        /// </summary>
        public bool IsFalse => IsValid && !Value;


        public static  BoolParameter FromPropertyName(string propertyName, Type inType)
        {
            return (BoolParameter)ValueTypeParameter<bool>.FromPropertyName(propertyName, inType);
        }
    }
}
