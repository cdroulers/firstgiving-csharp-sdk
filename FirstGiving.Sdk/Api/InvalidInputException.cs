using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FirstGiving.Sdk.Api
{
    public class InvalidInputException : FirstGivingException
    {
        public string ErrorTarget { get; private set; }

        public InvalidInputException(Exception innerException, XDocument responseContent)
            : base(GetErrorMessage(responseContent), innerException, responseContent)
        {
        }

        private static string GetErrorMessage(XDocument responseContent)
        {
            var node = responseContent.Descendants("firstGivingResponse").First();
            string errorMessage = node.Attributes("verboseErrorMessage").First().Value;
            var target = node.Attributes("errorTarget").FirstOrDefault();
            string errorTarget = target == null ? string.Empty : target.Value;
            return string.Format(@"An error occurred for the target ""{0}"". Error message was:
{1}", errorTarget, errorMessage);
        }
    }
}
