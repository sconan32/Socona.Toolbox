
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socona.ToolBox.Parametrization.Parameters
{
    public sealed class NullParameter : IParameter
    {
        private NullParameter()
        {

        }

        public static NullParameter Instance = new NullParameter();

        string IParameter.Name => throw new NotImplementedException();



        object IParameter.GivenValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        object IParameter.DefaultValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        string IParameter.Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        OptionAttribute IParameter.Option => throw new NotImplementedException();

        object IParameter.Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        IEnumerable IParameter.Candidates => throw new NotImplementedException();

        IReadOnlyCollection<ValidationAttribute> IParameter.Constraints => throw new NotImplementedException();

        bool IParameter.HasDefaultValue => throw new NotImplementedException();

        bool IParameter.IsDefaultValue => throw new NotImplementedException();

        bool IParameter.IsEmpty => throw new NotImplementedException();

        bool IParameter.IsRequired => throw new NotImplementedException();

        bool IParameter.IsOptional => throw new NotImplementedException();

        bool IParameter.IsValid => throw new NotImplementedException();

        public string PlaceHolder => "<void>";

        public string FullName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      

        string IParameter.GetFullDescription()
        {
            throw new NotImplementedException();
        }

        void IParameter.AddConstraint(ValidationAttribute va)
        {
            throw new NotImplementedException();
        }
    }
}
