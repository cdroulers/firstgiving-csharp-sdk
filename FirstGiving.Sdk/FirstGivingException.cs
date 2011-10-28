using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FirstGiving.Sdk
{
    public class FirstGivingException : ApplicationException
    {
        public XDocument ResponseContent { get; private set; }

        public FirstGivingException(string message, Exception innerException, XDocument responseContent)
            : base(message, innerException)
        {
            this.ResponseContent = responseContent;
        }
    }
}
