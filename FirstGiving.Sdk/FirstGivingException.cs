using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FirstGiving.Sdk
{
    /// <summary>
    ///     A generic exception from FirstGiving.
    /// </summary>
    public class FirstGivingException : ApplicationException
    {
        /// <summary>
        /// Gets the content of the response.
        /// </summary>
        /// <value>
        /// The content of the response.
        /// </value>
        public XDocument ResponseContent { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstGivingException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="responseContent">Content of the response.</param>
        public FirstGivingException(string message, Exception innerException, XDocument responseContent)
            : base(message, innerException)
        {
            this.ResponseContent = responseContent;
        }
    }
}
