using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace Socona.ToolBox.Parametrization.Parameters
{
    public abstract class ListParameter<T> : Parameter<List<T>>
    {
        ///<summary>
        /// A pattern defining a &quot,&quot.
        ///</summary>
        public static readonly Regex SPLIT = new Regex(",");

        ///<summary>
        /// List separator character - &quot;:&quot;
        ///</summary>
        public static readonly String LIST_SEP = ",";

        ///<summary>
        /// A pattern defining a &quot:&quot.
        ///</summary>
        public static readonly Regex VECTOR_SPLIT = new Regex(":");

        ///<summary>
        /// Vector separator character - &quot;:&quot;
        ///</summary>
        public static readonly String VECTOR_SEP = ":";

        ///<summary>
        /// Constructs a list parameter with the given optionID.
        /// 
        ///   </summary>
        ///<param name="optionID">the unique id of this parameter</param>
        ///<param name="constraints">the constraints of this parameter, may be null</param>
        ///<param name="defaultValue">the default value of this parameter (may be null)</param>
        public ListParameter(OptionAttribute optionID, bool isRequired, List<T> defaultValue, List<T>[] candidates, params ValidationAttribute[] constraints)
            : base(optionID, isRequired, defaultValue, candidates, constraints)
        { }

        ///<summary>
        /// Returns the size of this list parameter.
        /// 
        /// </summary>
        ///<returns>the size of this list parameter.</returns>
        public virtual int GetListSize()
        {
            return Value == null ? 0 : Value.Count;
        }

        public override string ToString()
        {
            return "Parameter:" + m_option.Name + " Type: List-of" + typeof(T).Name;
        }

        ///<summary>
        /// Returns a string representation of this list parameter. The elements of
        /// this list parameters are given in &quot;[ ]&quot;, comma separated.
        ///</summary>

        // TODO: keep? remove?
        protected String AsString()
        {
            if (Value == null)
            {
                return "";
            }
            StringBuilder buffer = new StringBuilder();
            buffer.Append("[");

            for (int i = 0; i < Value.Count; i++)
            {
                buffer.Append(Value[i]);
                if (i != Value.Count - 1)
                {
                    buffer.Append(",");
                }
            }
            buffer.Append("]");
            return buffer.ToString();
        }
    }
}