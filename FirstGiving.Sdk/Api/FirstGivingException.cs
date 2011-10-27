using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstGiving.Sdk.Api
{
    public class FirstGivingException : ApplicationException
    {
        public FirstGivingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
