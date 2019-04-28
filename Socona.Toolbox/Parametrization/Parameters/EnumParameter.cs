using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socona.ToolBox.Parametrization.Parameters
{
    ///<summary>
    /// Parameter class for a parameter specifying an enum type.
    /// 
    /// <para>
    /// Usage:
    /// 
    /// <pre>
    /// // Enum declaration.
    /// enum MyEnum  VALUE1, VALUE2 ;
    /// // Parameter value holder.
    /// MyEnum myEnumParameter;
    /// 
    /// // ...
    /// 
    /// // Parameterization.
    /// EnumParameter&lt;MyEnum&gt; param = new EnumParameter&lt;MyEnum&gt;(ENUM_PROPERTY_ID, MyEnum.class);
    /// // OR
    /// EnumParameter&lt;MyEnum&gt; param = new EnumParameter&lt;MyEnum&gt;(ENUM_PROPERTY_ID, MyEnum.class, MyEnum.VALUE1);
    /// // OR
    /// EnumParameter&lt;MyEnum&gt; param = new EnumParameter&lt;MyEnum&gt;(ENUM_PROPERTY_ID, MyEnum.class, true);
    /// 
    /// if(config.grab(param)) 
    ///   myEnumParameter = param.getValue();
    /// 
    /// 
    /// </para>
    /// </summary>
    ///<typeparam name="TE">Enum type</typeparam>
    public class EnumParameter<TE> : ValueTypeParameter<TE>
        where TE : struct, Enum
    {
        ///<summary>
        /// Reference to the actual enum type, for T.valueOf().
        ///</summary>
        protected Type enumClass;

        ///<summary>
        /// Constructs an enum parameter with the given optionID, constraints and
        /// default value.
        ///  </summary>
        ///<param name="optionID">the unique id of the parameter</param>
        ///<param name="defaultValue">the default value of the parameter</param>
        public EnumParameter(OptionAttribute optionID, bool isRequired, TE? defaultValue) :
            base(optionID, isRequired, defaultValue)
        {
            this.enumClass = typeof(TE);
        }
                     
        public override string PlaceHolder => $"<{JoinEnumNames(" | ")}>";
        
        protected override bool TryParse(object obj, out TE value)
        {
            if (obj is TE teEnum)
            {
                value = teEnum;
                return true;
            }
            if (enumClass.IsInstanceOfType(obj))
            {
                value = (TE)obj;
                return true;
            }
            if (obj is string valStr)
            {
                return Enum.TryParse<TE>(valStr, out value);
            }
            value = default;
            return false;
        }                  

        ///<summary>
        /// Get a list of possible values for this enum parameter.
        /// 
        /// </summary>
        ///<returns>list of strings representing possible enum values.</returns>
        public ICollection<String> GetPossibleValues()
        {
            // Convert to string array
            return Enum.GetNames(enumClass);
        }

        ///<summary>
        /// Utility method for merging possible values into a string for informational
        /// messages.
        /// 
        ///  </summary>
        ///<param name="separator">char sequence to use as a separator for enum values.</param>
        ///<returns><code>VAL1separatorVAL2separator...</code></returns>
        private String JoinEnumNames(String separator)
        {
            string[] enumTypes = Enum.GetNames(enumClass);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < enumTypes.Length; ++i)
            {
                if (i > 0)
                {
                    sb.Append(separator);
                }
                sb.Append(enumTypes[i]);
            }
            return sb.ToString();
        }
    }
}
