using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FirstGiving.Sdk
{
    /// <summary>
    ///     Response data from FirstGiving API query.
    /// </summary>
    public class ResponseData
    {
        /// <summary>
        /// Gets the body.
        /// </summary>
        public XDocument Body { get; private set; }
        /// <summary>
        /// Gets the original response.
        /// </summary>
        public string OriginalResponse { get; private set; }
        /// <summary>
        /// Gets the signature.
        /// </summary>
        public string Signature { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseData"/> class.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="originalResponse">The original response.</param>
        /// <param name="signature">The signature.</param>
        public ResponseData(XDocument body, string originalResponse, string signature)
        {
            this.Body = body;
            this.OriginalResponse = originalResponse;
            this.Signature = signature;
        }
    }
}
