using Socona.ToolBox.Parametrization.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socona.ToolBox.Parametrization
{
    public class ParameterException : Exception
    {
        public ParameterException(String message)
            : base(message)
        { }

        public ParameterException(String message, Exception inner)
            : base(message, inner)
        { }

    }

    public class InvalidParameterValueException : ParameterException
    {
        ///<summary>
        /// Thrown by a Parameterizable object in case of wrong parameter format.
        ///</summary>
        ///<param name="parameter">the parameter that has a wrong value</param>
        ///<param name="value">the value of the parameter read by the option handler</param>        
        ///<param name="inner">the cause</param>
        public InvalidParameterValueException(IParameter parameter, string @value = null, string message = null, Exception inner = null) :
            this(BuildExceptionMessage(parameter, @value, message), inner)
        {
        }

        private static string BuildExceptionMessage(IParameter para, string @value, string message)
        {
            return $"Wrong value of parameter {para.Name}.\n Value:{@value}\n Message:{message}\n Expected: {para.GetFullDescription()}";
        }
        public InvalidParameterValueException(String message) : base(message) { }

        public InvalidParameterValueException(String message, Exception e) : base(message, e) { }
    }
}
