using Socona.ToolBox.Parametrization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Socona.ToolBox.Parametrization.Parameters
{
    public interface IParameter
    {
        string Name { get; }

        string FullName { get; set; }

        object GivenValue { get; set; }

        object DefaultValue { get; set; }

        string Description { get; set; }

        string PlaceHolder { get; }

        OptionAttribute Option { get; }

        object Value { get; set; }

        IEnumerable Candidates { get; }

        IReadOnlyCollection<ValidationAttribute> Constraints { get; }

        bool HasDefaultValue { get; }

        bool IsDefaultValue { get; }

        bool IsEmpty { get; }

        bool IsRequired { get; }

        bool IsOptional { get; }

        bool IsValid { get; }

        string GetFullDescription();

        void AddConstraint(ValidationAttribute va);
    }
}
