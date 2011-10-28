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
    public abstract class BaseRestClient
    {
        public Uri ApiEndpoint { get; private set; }

        protected BaseRestClient(Uri apiEndpoint)
        {
            this.ApiEndpoint = apiEndpoint;
        }

        protected abstract void PreRequest(HttpWebRequest request);
        protected abstract void PostRequest(string result, XDocument resultBody, HttpWebResponse response);
        protected abstract void OnException(XDocument resultBody, HttpWebResponse response, Exception e);

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

        protected XDocument SendApiRequest(string resourceName, string httpMethod, IDictionary<string, string> values = null)
        {
            Validate.Is.NotNullOrWhiteSpace(resourceName, "resourceName");
            Validate.Is.NotNullOrWhiteSpace(httpMethod, "httpMethod");

            HttpWebRequest request = null;

            switch (httpMethod.ToUpperInvariant())
            {
                case "GET":
                    request = this.SetUpGetRequest(resourceName, values);
                    break;
                case "POST":
                    request = this.SetUpPostRequest(resourceName, values);
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
                        string xmlResponse = reader.ReadToEnd();
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

        private HttpWebRequest SetUpGetRequest(string resourceName, IDictionary<string, string> values)
        {
            var uri = this.GetResourceUri(resourceName, values);
            var request = HttpWebRequest.Create(uri) as HttpWebRequest;
            request.Method = "GET";
            this.PreRequest(request);

            return request;
        }

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
    }
}
