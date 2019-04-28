using System;


namespace Socona.ToolBox.Parametrization.Parameters
{
    public class ObjectParameter<TC> : Parameter<TC>
    {
        ///<summary>
        /// The instance to use.
        ///</summary>

        private TC instance;

        ///<summary>
        /// Constructs a class parameter with the given optionID, restriction class,
        /// and default value.
        /// 
        ///   </summary>
        ///<param name="optionID">the unique id of the option</param>
        ///<param name="restrictionClass">the restriction class of this class parameter</param>
        ///<param name="defaultValue">the default value of this class parameter</param>

        public ObjectParameter(OptionAttribute optionID, bool isRequired = false, Type defaultValue = null)
            : base(optionID, typeof(TC), isRequired, defaultValue)
        { }



        protected override bool ParseValue(object obj, out TC value)
        {
            if (obj is TC valueTC)
            {
                value = valueTC;
                return true;
            }
            if (baseType.IsInstanceOfType(obj))
            {
                value = (TC)obj;
                return true;
            }
          
        }

        ///<summary> @inheritDoc </summary>


        public override void SetValue(Object obj)
        {
            // This is a bit hackish. But when given an appropriate instance, keep it.
            if (restrictionClass.IsInstanceOfType(obj))
            {
                instance = (TC)obj;
            }
            base.SetValue(obj);
        }

        ///<summary>
        /// Returns a string representation of the parameter's type.
        /// 
        /// </summary>
        ///<returns>&quot;&lt;class&gt;&quot;</returns>

        public override String GetSyntax()
        {
            return "<class|object>";
        }

        ///<summary>
        /// Returns a new instance for the value (i.e., the class name) of this class
        /// parameter. The instance has the type of the restriction class of this class
        /// parameter.
        /// <br/>
        /// If the Class for the class name is not found, the instantiation is tried
        /// using the package of the restriction class as package of the class name.
        /// 
        ///  </summary>
        ///<param name="config">Parameterization</param>
        ///<returns>a new instance for the value of this class parameter</returns>

        public TC InstantiateClass(IParameterization config)
        {
            if (instance != null)
            {
                return instance;
            }
            // NOTE: instance may be null here, when instantiateClass failed.
            return instance = base.InstantiateClass<TC>(config);
        }

        ///<summary> @inheritDoc </summary>


        public override Object GetGivenValue()
        {
            if (instance != null)
            {
                return instance;
            }
            return base.GetGivenValue();
        }
    }
}