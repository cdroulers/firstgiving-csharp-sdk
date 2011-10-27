using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstGiving.Sdk.Api
{
    public class InvalidInputException : FirstGivingException
    {
        public string ErrorTarget { get; private set; }

        public InvalidInputException(string errorMessage, string errorTarget, Exception innerException)
            : base(string.Format(@"An error occurred for the target ""{0}"". Error message was:
{1}", errorTarget, errorMessage), innerException)
        {
        }
    }
}
