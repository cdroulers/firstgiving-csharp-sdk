using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FirstGiving.Sdk
{
    /// <summary>
    /// An exception on the server-side.
    /// </summary>
    public class ServerErrorException : FirstGivingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerErrorException"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="responseContent">Content of the response.</param>
        public ServerErrorException(string errorMessage, Exception innerException, XDocument responseContent)
            : base(errorMessage, innerException, responseContent)
        {
        }
    }
}
