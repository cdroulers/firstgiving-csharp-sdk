using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FirstGiving.Sdk
{
    /// <summary>
    /// When the query was not authorized
    /// </summary>
    public class UnauthorizedException : FirstGivingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedException"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="secret">The secret.</param>
        /// <param name="inner">The inner.</param>
        /// <param name="responseContent">Content of the response.</param>
        public UnauthorizedException(string key, string secret, Exception inner, XDocument responseContent)
            : base(string.Format("You may not access the API with an application ID \"{0}\" and the secret \"{1}\".", secret, key), inner, responseContent)
        {
        }
    }
}
