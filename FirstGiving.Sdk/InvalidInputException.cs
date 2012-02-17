using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FirstGiving.Sdk
{
    /// <summary>
    ///     If the input was invalid.
    /// </summary>
    public class InvalidInputException : FirstGivingException
    {
        /// <summary>
        /// Gets the error target.
        /// </summary>
        public string ErrorTarget { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidInputException"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="responseContent">Content of the response.</param>
        /// <param name="errorTarget">The error target.</param>
        public InvalidInputException(string errorMessage, Exception innerException, XDocument responseContent, string errorTarget = null)
            : base(errorMessage, innerException, responseContent)
        {
            this.ErrorTarget = errorTarget;
        }
    }
}
