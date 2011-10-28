using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FirstGiving.Sdk
{
    public class InvalidInputException : FirstGivingException
    {
        public string ErrorTarget { get; private set; }

        public InvalidInputException(string errorMessage, Exception innerException, XDocument responseContent, string errorTarget = null)
            : base(errorMessage, innerException, responseContent)
        {
            this.ErrorTarget = errorTarget;
        }
    }
}
