using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Web;

namespace FirstGiving.Sdk
{
    /// <summary>
    /// A basic rest client for API queries
    /// </summary>
    public abstract class BaseRestClient
    {
        /// <summary>
        /// Gets the API endpoint.
        /// </summary>
        public Uri ApiEndpoint { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRestClient"/> class.
        /// </summary>
        /// <param name="apiEndpoint">The API endpoint.</param>
        protected BaseRestClient(Uri apiEndpoint)
        {
            this.ApiEndpoint = apiEndpoint;
        }

        /// <summary>
        /// Called just before a request is made.
        /// </summary>
        /// <param name="request">The request.</param>
        protected abstract void PreRequest(HttpWebRequest request);
        /// <summary>
        /// Called after a request is made.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="resultBody">The result body.</param>
        /// <param name="response">The response.</param>
        protected abstract void PostRequest(string result, XDocument resultBody, HttpWebResponse response);
        /// <summary>
        /// Called when an exception occurss
        /// </summary>
        /// <param name="resultBody">The result body.</param>
        /// <param name="response">The response.</param>
        /// <param name="e">The e.</param>
        protected abstract void OnException(XDocument resultBody, HttpWebResponse response, Exception e);

        /// <summary>
        /// Gets the resource URI.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        protected Uri GetResourceUri(string resourceName, IDictionary<string, string> values = null)
        {
            var builder = new UriBuilder(this.ApiEndpoint);
            builder.Path += (resourceName.StartsWith("/") ? resourceName : "/" + resourceName);
            if (values != null)
            {
                builder.Query = string.Join("&", values.Select(v => v.Key + "=" + HttpUtility.UrlEncode(v.Value)));
            }
            return builder.Uri;
        }

        /// <summary>
        /// Sends the API request.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        protected XDocument SendApiRequest(string resourceName, string httpMethod, IDictionary<string, string> values = null)
        {
            Validate.Is.Not.NullOrWhiteSpace(resourceName, "resourceName");
            Validate.Is.Not.NullOrWhiteSpace(httpMethod, "httpMethod");

            HttpWebRequest request = null;

            switch (httpMethod.ToUpperInvariant())
            {
                case "GET":
                    request = this.SetUpGetRequest(resourceName, values);
                    break;
                case "POST":
                    request = this.SetUpPostRequest(resourceName, values);
                    break;
                case "DELETE":
                    request = this.SetUpDeleteRequest(resourceName, values);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The HTTP method \"{0}\" is not supported", httpMethod));
            }

            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        string xmlResponse = SanitizeXmlString(reader.ReadToEnd());
                        var result = XDocument.Parse(xmlResponse);
                        this.PostRequest(xmlResponse, result, response);
                        return result;
                    }
                }
            }
            catch (WebException e)
            {
                using (var response = (HttpWebResponse)e.Response)
                {
                    string xmlResponse = null;
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        xmlResponse = reader.ReadToEnd();
                    }
                    var result = XDocument.Parse(xmlResponse);
                    this.PostRequest(xmlResponse, result, response);
                    this.OnException(result, response, e);
                }
                throw;
            }
        }

        /// <summary>
        /// Sets up get request.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        private HttpWebRequest SetUpGetRequest(string resourceName, IDictionary<string, string> values)
        {
            var uri = this.GetResourceUri(resourceName, values);
            var request = HttpWebRequest.Create(uri) as HttpWebRequest;
            request.Method = "GET";
            this.PreRequest(request);

            return request;
        }

        /// <summary>
        /// Sets up delete request.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        private HttpWebRequest SetUpDeleteRequest(string resourceName, IDictionary<string, string> values)
        {
            var request = this.SetUpGetRequest(resourceName, values);
            request.Method = "DELETE";
            return request;
        }

        /// <summary>
        /// Sets up post request.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        private HttpWebRequest SetUpPostRequest(string resourceName, IDictionary<string, string> values)
        {
            var uri = this.GetResourceUri(resourceName);
            var request = HttpWebRequest.Create(uri) as HttpWebRequest;
            request.Method = "POST";
            this.PreRequest(request);

            if (values != null)
            {
                string content = string.Join("&", values.Select(v => v.Key + "=" + v.Value));
                byte[] byteContent = Encoding.UTF8.GetBytes(content);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteContent.Length;
                var dataStream = request.GetRequestStream();
                dataStream.Write(byteContent, 0, byteContent.Length);
                dataStream.Close();
            }
            return request;
        }

        /// <summary>
        /// Remove illegal XML characters from a string.
        /// </summary>
        private static string SanitizeXmlString(string xml)
        {
            Validate.Is.Not.Null(xml, "xml");

            StringBuilder buffer = new StringBuilder(xml.Length);

            foreach (char c in xml)
            {
                if (IsLegalXmlChar(c))
                {
                    buffer.Append(c);
                }
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Whether a given character is allowed by XML 1.0.
        /// </summary>
        private static bool IsLegalXmlChar(int character)
        {
            return
            (
                 character == 0x9 /* == '\t' == 9   */          ||
                 character == 0xA /* == '\n' == 10  */          ||
                 character == 0xD /* == '\r' == 13  */          ||
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF)
            );
        }
    }
}
