using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Socona.ToolBox.Validation
{
    public class FutureDateTimeAttribute : ValidationAttribute
    {
        public DateTime CurrentTime { get; private set; }

        public FutureDateTimeAttribute()
        {
            CurrentTime = DateTime.Now;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value is DateTime dateTime)
            {

                // at this stage you have "value" and "otherValue" pointing
                // to the value of the property on which this attribute
                // is applied and the value of the other property respectively
                // => you could do some checks
                if (dateTime > CurrentTime)
                {
                    // here we are verifying whether the 2 values are equal
                    // but you could do any custom validation you like
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult(this.FormatErrorMessage(validationContext?.DisplayName ?? string.Empty)); ;
        }

    }
}
