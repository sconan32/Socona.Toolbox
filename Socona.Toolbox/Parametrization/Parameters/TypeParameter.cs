using Socona.ToolBox.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;


namespace Socona.ToolBox.Parametrization.Parameters
{
    public class TypeParameter : Parameter<Type>
    {

        ///<summary>
        /// The restriction class for this class parameter.
        ///</summary>
        protected Type baseType;

        ///<summary>
        /// Constructs a class parameter with the given optionID, restriction class,
        /// and default value.
        /// 
        ///   </summary>
        ///<param name="optionID">the unique id of the option</param>
        ///<param name="restrictionClass">the restriction class of this class parameter</param>
        ///<param name="defaultValue">the default value of this class parameter</param>

        public TypeParameter(OptionAttribute optionID, Type baseType = null, bool isRequired = false, Type defaultValue = null, Type[] candidates = null,
            params ValidationAttribute[] constraints) :
            base(optionID, isRequired, defaultValue, candidates, constraints)
        {
            this.baseType = baseType;
            if (this.baseType == null)
            {
                this.baseType = typeof(object);
            }
        }

        protected override bool TryParse(object obj, out Type value)
        {
            if (obj is Type objType)
            {
                value = objType;
                return true;
            }
            if (obj is string valueString)
            {
                try
                {   // Try exact class factory first.
                    try
                    {
                        value = Type.GetType(valueString, true);
                        return true;
                    }
                    catch (TypeLoadException)
                    {
                        // Ignore, retry
                    }
                    // Last try: guessed name prefix only
                    value = Type.GetType(baseType.Namespace + "." + valueString);
                    return true;
                }
                catch (TypeLoadException)
                {
                    // Ignore,  will return False
                }
            }
            value = null;
            return false;
        }

        ///<summary>
        /// Checks if the given parameter value is valid for this ClassParameter. If
        /// not a parameter exception is thrown.
        ///</summary>
        protected override bool ValidateConstraints(Type obj, bool throwIfFailed = true)
        {
            if (obj == null)
            {
                if (throwIfFailed)
                {
                    throw new InvalidParameterValueException("Parameter Error.\n" + "No value for parameter \"" + Name + "\" " + "given.");
                }
                return false;
            }
            if (!baseType.IsAssignableFrom(obj))
            {
                if (throwIfFailed)
                {
                    throw new InvalidParameterValueException(this, obj.Name, "Given class not a subclass / implementation of " + baseType.Name);
                }
                return false;
            }

            return base.ValidateConstraints(obj, throwIfFailed);
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
        ///<param name="config">Parameterization to use (if Parameterizable))</param>
        ///<returns>a new instance for the value of this class parameter</returns>
        public bool TryInstantiate<TC>(ParametrizationContext config, out TC value)
        {
            //    try
            //    {
            //        if (Value == null /* && !optionalParameter */)
            //        {
            //            throw new UnusedParameterException("Value of parameter " + GetName() + " has not been specified.");
            //        }
            //        C instance;
            //        try
            //        {
            //            config = config.Descend(this);
            //            instance = (C)ClassGenericsUtil.TryInstantiate(GetValue(), config);
            //        }
            //        //catch (TargetInvocationException e)
            //        //{
            //        //    // inner exception during instantiation. Log, so we don't lose it!
            //        //    logger.Error(e);
            //        //    throw new WrongParameterValueException(this, GetValue().FullName, "Error instantiating class.", e);
            //        //}
            //        //catch (MissingMethodException )
            //        //{
            //        //    throw new WrongParameterValueException(this, GetValue().FullName, "Error instantiating class - no usable public constructor.");
            //        //}
            //        catch (Exception e)
            //        {
            //            throw new WrongParameterValueException(this, GetValue().FullName, "Error instantiating class.", e);
            //        }
            //        return instance;
            //    }
            //    catch (ParameterException e)
            //    {
            //        //config.ReportError(e);
            //        return default(C);
            //    }
            value = default;
            return false;
        }

        ///<summary>
        /// Returns the restriction class of this class parameter.
        /// </summary>
        public Type BaseType => baseType;


        ///<summary>
        /// RawAt an iterator over all known implementations of the class restriction.
        /// </summary>
        ///<returns>List object</returns>
        public IEnumerable<Type> GetKnownImplementations()
        {
            return baseType.GetAllImplementations();
        }

        ///<summary>
        /// Provides a description string listing all classes for the given superclass
        /// or interface as specified in the properties.
        /// 
        /// </summary>
        ///<returns>a description string listing all classes for the given superclass
        ///         or interface as specified in the properties</returns>
        public String RestrictionString()
        {
            StringBuilder info = new StringBuilder();
            if (baseType.IsInterface)
            {
                info.Append("Implementing ");
            }
            else
            {
                info.Append("Extending ");
            }
            info.Append(baseType.Name);
            info.Append(Environment.NewLine);

            var known = GetKnownImplementations();
            if (known.Any())
            {
                info.Append("Known classes (default package " + baseType.Assembly.FullName + "):");
                info.Append(Environment.NewLine);
                foreach (Type c in known)
                {
                    info.Append("-> ");
                    info.Append(c.FullName);
                    info.Append(Environment.NewLine);
                }
            }
            return info.ToString();
        }


    }
}
