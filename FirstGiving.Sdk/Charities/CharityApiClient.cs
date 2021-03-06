﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Data.Json;
using System.Globalization;

namespace FirstGiving.Sdk.Charities
{
    /// <summary>
    ///     Basic implementation of ICharityApiClient
    /// </summary>
    public class CharityApiClient : BaseRestClient, ICharityApiClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharityApiClient"/> class.
        /// </summary>
        /// <param name="apiEndpoint">The API endpoint.</param>
        public CharityApiClient(Uri apiEndpoint)
            : base(apiEndpoint)
        {
        }

        /// <summary>
        /// Gets a charity by FirstGiving UUID.
        /// </summary>
        /// <param name="uuid">The UUID.</param>
        /// <returns></returns>
        public Charity GetByUUID(Guid uuid)
        {
            var xml = this.SendApiRequest(string.Format("/v1/object/organization/{0}", uuid), "GET");
            var payload = xml.Descendants("payload").Last();
            return CharityApiClient.GetCharity(payload);
        }

        /// <summary>
        /// Gets a charity by government ID.
        /// </summary>
        /// <param name="governmentID">The government ID.</param>
        /// <returns></returns>
        public Charity GetByGovernmentID(string governmentID)
        {
            var parameters = new Dictionary<string, string>()
            {
                { "q", string.Format("government_id:{0}", governmentID) }
            };
            var xml = this.SendApiRequest("/v1/list/organization", "GET", parameters);
            var results = CharityApiClient.GetCharities(xml.Descendants("payload").First());

            if (!results.Any())
            {
                throw new NotFoundException(string.Format("Charity with Government ID \"{0}\" not found.", governmentID), new ApplicationException("Query returned no results!"), xml);
            }
            return results.First();
        }

        /// <summary>
        /// Finds a charity by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Charity[] FindByName(string name)
        {
            var parameters = new Dictionary<string, string>()
            {
                { "q", string.Format("organization_name:{0}", name) }
            };
            var xml = this.SendApiRequest("/v1/list/organization", "GET", parameters);
            var results = CharityApiClient.GetCharities(xml.Descendants("payload").First());

            return results.ToArray();
        }

        /// <summary>
        /// Called just before a request is made.
        /// </summary>
        /// <param name="request">The request.</param>
        protected override void PreRequest(HttpWebRequest request)
        {
        }

        /// <summary>
        /// Called after a request is made.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="resultBody">The result body.</param>
        /// <param name="response">The response.</param>
        protected override void PostRequest(string result, XDocument resultBody, HttpWebResponse response)
        {
        }

        /// <summary>
        /// Called when an exception occurss
        /// </summary>
        /// <param name="resultBody">The result body.</param>
        /// <param name="response">The response.</param>
        /// <param name="e">The e.</param>
        protected override void OnException(XDocument resultBody, HttpWebResponse response, Exception e)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    throw new NotFoundException("Object not found.", e, resultBody);
                case HttpStatusCode.BadRequest:
                    throw new InvalidInputException(GetErrorMessage(resultBody), e, resultBody);
                case HttpStatusCode.InternalServerError:
                    throw new ServerErrorException(GetErrorMessage(resultBody), e, resultBody);
            }
        }
        
        private static string GetErrorMessage(XDocument responseContent)
        {
            var node = responseContent.Descendants("message").First();
            return node.Value;
        }

        private static List<Charity> GetCharities(XElement element)
        {
            return element.Descendants().Where(x => x.Name.LocalName.StartsWith("key_")).Select(c => CharityApiClient.GetCharity(c)).ToList();
        }

        private static Charity GetCharity(XElement element)
        {
            Uri uri = null;
            var url = Uri.TryCreate(element.Descendants("url").First().Value, UriKind.Absolute, out uri) ? uri : null;
            return new Charity(
                Guid.Parse(element.Descendants("organization_uuid").First().Value),
                element.Descendants("organization_name").First().Value,
                element.Descendants("organization_alias").First().Value,
                element.Descendants("government_id").First().Value,
                element.Descendants("address_line_1").First().Value,
                element.Descendants("address_line_2").First().Value,
                element.Descendants("address_line_3").First().Value,
                element.Descendants("address_line_full").First().Value,
                element.Descendants("address_full").First().Value,
                element.Descendants("city").First().Value,
                element.Descendants("region").First().Value,
                element.Descendants("postal_code").First().Value,
                element.Descendants("county").First().Value,
                element.Descendants("country").First().Value,
                element.Descendants("phone_number").First().Value,
                element.Descendants("area_code").First().Value,
                url,
                element.Descendants("category_code").First().Value,
                double.Parse(element.Descendants("latitude").First().Value, CultureInfo.InvariantCulture),
                double.Parse(element.Descendants("longitude").First().Value, CultureInfo.InvariantCulture),
                element.Descendants("revoked").First().Value == "1"
            );
        }
    }
}
