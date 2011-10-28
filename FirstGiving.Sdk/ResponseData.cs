using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FirstGiving.Sdk
{
    public class ResponseData
    {
        public XDocument Body { get; private set; }
        public string OriginalResponse { get; private set; }
        public string Signature { get; private set; }

        public ResponseData(XDocument body, string originalResponse, string signature)
        {
            this.Body = body;
            this.OriginalResponse = originalResponse;
            this.Signature = signature;
        }
    }
}
