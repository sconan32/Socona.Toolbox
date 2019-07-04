using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Socona.ToolBox.Validation
{
    public class LessThanAttribute : ValidationAttribute
    {

        string _otherPropertyName;

        public override bool RequiresValidationContext => true;

        public LessThanAttribute(string otherPropertyName)
        {
            _otherPropertyName = otherPropertyName;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (otherProperty != null)
            {
                var propertyValue = otherProperty.GetValue(validationContext.ObjectInstance);

                if (propertyValue != null)
                {
                    if (value is IComparable lhs && propertyValue is IComparable rhs)
                    {
                        if (lhs.CompareTo(rhs) < 0)
                        {
                            return ValidationResult.Success;
                        }
                    }
                }
            }
            return new ValidationResult(this.FormatErrorMessage(validationContext?.DisplayName ?? string.Empty)); ;
        }
    }
}
