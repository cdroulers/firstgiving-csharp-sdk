using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FirstGiving.Sdk
{
    /// <summary>
    ///     Exception for when a charity is not found.
    /// </summary>
    public class NotFoundException : FirstGivingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="responseContent">Content of the response.</param>
        public NotFoundException(string errorMessage, Exception innerException, XDocument responseContent)
            : base(errorMessage, innerException, responseContent)
        {
        }
    }
}
