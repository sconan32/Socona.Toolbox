using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socona.ToolBox.Exceptions
{
    /**
    * Exception for aborting some process and transporting a message.
    * 
    */
    [Serializable]
    public class AbortException : Exception
    {
        /**
         * Serial UID.
         */
     
        /**
         * Exception for aborting some process and transporting a message.
         * 
         * @param message message to be transported
         */
        public AbortException(String message)
            : base(message)
        {

        }

        /**
         * Exception for aborting some process and transporting a message.
         * 
         * @param message message to be transported
         * @param cause cause of this exception
         */
        public AbortException(String message, Exception cause) :
            base(message, cause)
        {
        }
    }

}
