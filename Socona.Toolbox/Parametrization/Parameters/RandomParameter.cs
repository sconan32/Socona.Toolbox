using Socona.ToolBox.Parametrization;
using Socona.ToolBox.Parametrization.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socona.Expor.Utilities.Options.Parameters
{
    ///<summary>
    /// Parameter for random generators and/or random seeds. 
    /// @author Erich Schubert
    ///</summary>

    public class RandomParameter : Parameter<Random>
    {
        private static readonly Random _DefaultValue = new Random(-1);
        ///<summary>
        /// Seed value, if used
        ///</summary>

        int seed = 0;

        ///<summary>
        /// Constructor with default value. The default value may be <code>null</code>,
        /// which means a new random will be generated.
        ///  </summary>
        ///<param name="optionID">Option ID</param>
        ///<param name="defaultValue">Default value. If <code>null</code>, a new random object
        ///        will be created.</param>
        public RandomParameter(OptionAttribute optionID, bool isRequired, Random defaultValue, Random[] candidates, ValidationAttribute[] constraints)
            : base(optionID, isRequired, defaultValue, candidates, constraints)
        { }

        public override string PlaceHolder => "<integer|Random>";



        protected override bool TryParse(object obj, out Random value)
        {
            if (obj is Random random)
            {
                value = random;
                return true;
            }
            if (obj is int vali1)
            {
                seed = vali1;
                value = new Random(vali1);
                return true;
            }
            if (int.TryParse(obj.ToString(), out int vali2))
            {
                seed = vali2;
                value = new Random(vali2);
                return true;
            }
            value = null;
            return false;
        }



        public override string GetValueAsString()
        {
            return Value?.ToString() ?? "null";
        }


        public override string GetDefaultValueAsString()
        {
            if (defaultValue == _DefaultValue)
            {
                return "<Global Random>";
            }
            return base.GetDefaultValueAsString();
        }
    }

}
