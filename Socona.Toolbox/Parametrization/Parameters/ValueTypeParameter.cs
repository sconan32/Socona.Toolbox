
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;



namespace Socona.ToolBox.Parametrization.Parameters
{
    public abstract class ValueTypeParameter<T> : Parameter<T>
    where T : struct
    {
        protected bool _hasDefaultValue = false;

        ///<summary>
        /// Constructs a number parameter with the given optionID, constraint, and
        /// optional flag.
        /// 
        ///   </summary>
        ///<param name="optionID">the unique id of this parameter</param>
        ///<param name="constraints">the constraints of this parameter</param>
        ///<param name="defaultValue">the default value for this parameter</param>
        public ValueTypeParameter(OptionAttribute optionID, bool isRequired = false, T? defaultValue = null, T[] candidates = null, params ValidationAttribute[] constraints) :
            base(optionID, isRequired, defaultValue ?? default)
        {

            _hasDefaultValue = (defaultValue != null);
        }

        public override bool HasDefaultValue => _hasDefaultValue;

        public override T Value
        {
            get => _value;
            set
            {
                _value = value;
                IsValid = true;
                givenValue = value;
                isDefaultValue = _value.Equals(defaultValue);
            }
        }

        protected static ValueTypeParameter<T> FromPropertyName(string propertyName, Type inType)            
        {
            return (ValueTypeParameter<T>)ParameterFactory.Instance.FromPropertyName(propertyName, inType);
        }
    }

}
