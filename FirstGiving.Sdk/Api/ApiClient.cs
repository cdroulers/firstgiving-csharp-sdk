using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Globalization;
using System.Xml.Linq;

namespace FirstGiving.Sdk.Api
{
    public class ApiClient : IApiClient
    {
        public string ApplicationKey { get; private set; }
        public string SecurityToken { get; private set; }
        public Uri ApiEndpoint { get; private set; }

        public ResponseData LastestResponse { get; private set; }

        public ApiClient(string applicationKey, string securityToken, Uri apiEndpoint)
        {
            this.ApplicationKey = applicationKey;
            this.SecurityToken = securityToken;
            this.ApiEndpoint = apiEndpoint;
        }

        public string SayHello()
        {
            string result = this.SendApiRequest("/default/test/0", "GET");

            var xml = XDocument.Parse(result);
            var message = xml.Descendants("friendlyMessage").First();

            return message.Value;
        }

        /// <summary>
        /// Makes a donation by credit card.
        /// </summary>
        /// <param name="donation">The donation.</param>
        /// <param name="paymentData">The payment data.</param>
        /// <param name="remoteAddress">The remote address of the user making the donation (prevents fraud and stuff).</param>
        /// <returns>A Transaction ID uniquely identifying the transaction. Basically the receipt number.</returns>
        public string DonateByCreditCard(Donation donation, CreditCardPaymentData paymentData, IPAddress remoteAddress)
        {
            var parameters = ApiClient.GetCreditCardParameters(donation, paymentData, remoteAddress);

            string result = this.SendApiRequest("/donation/creditcard", "POST", parameters);

            var xml = XDocument.Parse(result);
            var transactionID = xml.Descendants("transactionId").First();
            return transactionID.Value;
        }

        /// <summary>
        /// Makes a donation by credit card that will be recurring.
        /// </summary>
        /// <param name="donation">The donation.</param>
        /// <param name="paymentData">The payment data.</param>
        /// <param name="remoteAddress">The remote address of the user making the donation (prevents fraud and stuff).</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="term">The term. Null means until the ends of time.</param>
        /// <returns>
        /// A Transaction ID uniquely identifying the transaction. Basically the receipt number.
        /// </returns>
        public string DonateByCreditCardRecurring(Donation donation, CreditCardPaymentData paymentData, IPAddress remoteAddress, BillingFrequency frequency, int? term)
        {
            var parameters = ApiClient.GetCreditCardParameters(donation, paymentData, remoteAddress);
            parameters["recurringBillingFrequency"] = ApiClient.GetBillingFrequencyCode(frequency);
            parameters["recurringBillingTerm"] = (term.HasValue ? term.Value : 0).ToString();

            string result = this.SendApiRequest("/donation/recurringcreditcardprofile", "POST", parameters);

            var xml = XDocument.Parse(result);
            var transactionID = xml.Descendants("recurringDonationProfileId").First();
            return transactionID.Value;
        }

        /// <summary>
        /// Verify a message that you have received actually originated from FirstGiving’s API.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="signature">The signature.</param>
        /// <returns>True if it originated from FirstGiving.</returns>
        public bool Verify(string message, string signature)
        {
            Validate.Is.NotNullOrWhiteSpace(message, "message");
            Validate.Is.NotNullOrWhiteSpace(signature, "signature");

            var parameters = new Dictionary<string, string>();
            parameters["message"] = message;
            parameters["signature"] = signature;

            var result = this.SendApiRequest("/verify", "POST", parameters);
            var xml = XDocument.Parse(result);
            var valid = xml.Descendants("valid").First();
            return valid.Value == "1";
        }

        protected Uri GetResourceUri(string resourceName)
        {
            var builder = new UriBuilder(this.ApiEndpoint);
            builder.Path += (resourceName.StartsWith("/") ? resourceName : "/" + resourceName);
            return builder.Uri;
        }

        protected string SendApiRequest(string resourceName, string httpMethod, IDictionary<string, string> values = null)
        {
            Validate.Is.NotNullOrWhiteSpace(resourceName, "resourceName");
            Validate.Is.NotNullOrWhiteSpace(httpMethod, "httpMethod");

            var uri = this.GetResourceUri(resourceName);
            var request = HttpWebRequest.Create(uri) as HttpWebRequest;

            request.Accept = "application/json";
            request.Headers["JG_APPLICATIONKEY"] = this.ApplicationKey;
            request.Headers["JG_SECURITYTOKEN"] = this.SecurityToken;

            switch (httpMethod.ToUpperInvariant())
            {
                case "GET":
                    ApiClient.SetUpGetRequest(request);
                    break;
                case "POST":
                    ApiClient.SetUpPostRequest(request, values);
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
                        string result = reader.ReadToEnd();
                        this.LastestResponse = new ResponseData() { Body = result, Signature = response.Headers["Jg-Response-Signature"] };
                        return result;
                    }
                }
            }
            catch (WebException e)
            {
                using (var response = (HttpWebResponse)e.Response)
                {
                    string result = null;
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                        this.LastestResponse = new ResponseData() { Body = result, Signature = response.Headers["Jg-Response-Signature"] };
                    }
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedException(this.ApplicationKey, this.SecurityToken, e, XDocument.Parse(result));
                        case HttpStatusCode.BadRequest:
                            throw new InvalidInputException(e, XDocument.Parse(result));
                        case HttpStatusCode.InternalServerError:
                            throw new ServerErrorException(e, XDocument.Parse(result));
                    }
                }
                throw;
            }
        }

        private static IDictionary<string, string> GetCreditCardParameters(Donation donation, CreditCardPaymentData paymentData, IPAddress remoteAddress)
        {
            var parameters = new Dictionary<string, string>();
            parameters["ccNumber"] = paymentData.CardNumber;
            parameters["ccType"] = ApiClient.GetCreditCardKindCode(paymentData.CardKind);
            parameters["ccExpDateMonth"] = paymentData.CardExpirationDate.Month.ToString();
            parameters["ccExpDateYear"] = paymentData.CardExpirationDate.Year.ToString();
            parameters["ccCardValidationNum"] = paymentData.CardValidationNumber;
            parameters["billToTitle"] = paymentData.Title;
            parameters["billToFirstName"] = paymentData.FirstName;
            parameters["billToMiddleName"] = paymentData.MiddleName;
            parameters["billToLastName"] = paymentData.LastName;
            parameters["billToAddressLine1"] = paymentData.Address1;
            parameters["billToAddressLine2"] = paymentData.Address2;
            parameters["billToAddressLine3"] = paymentData.Address3;
            parameters["billToCity"] = paymentData.City;
            parameters["billToState"] = paymentData.State;
            parameters["billToZip"] = paymentData.ZipCode;
            parameters["billToCountry"] = paymentData.Country;
            parameters["billToEmail"] = paymentData.EmailAddress.Address;
            parameters["billToPhone"] = paymentData.PhoneNumber;
            parameters["remoteAddr"] = remoteAddress.ToString();

            parameters["charityId"] = donation.CharityID.ToString();
            parameters["eventId"] = donation.EventID ?? "";
            parameters["fundraiserId"] = donation.FundraiserID.HasValue ? donation.FundraiserID.Value.ToString() : null;
            parameters["orderId"] = donation.OrderID.HasValue ? donation.OrderID.Value.ToString() : null;
            parameters["description"] = donation.Description;
            parameters["reportDonationToTaxAuthority"] = donation.ReportDonationToTaxAuthority ? "1" : "0";
            parameters["personalIdentificationNumber"] = string.IsNullOrWhiteSpace(donation.PersonalIdentificationNumber) ? "" : donation.PersonalIdentificationNumber;
            parameters["donationMessage"] = donation.DonationMessage;
            parameters["honorMemoryName"] = donation.HonorMemoryName;
            parameters["billingDescriptor"] = paymentData.BillingDescriptor;
            parameters["amount"] = donation.Amount.ToString("0.00", CultureInfo.InvariantCulture);
            parameters["currencyCode"] = ApiClient.GetCurrencyCode(donation.Currency);

            return parameters;
        }

        private static void SetUpGetRequest(WebRequest request)
        {
            request.Method = "GET";
        }

        private static void SetUpPostRequest(WebRequest request, IDictionary<string, string> values)
        {
            request.Method = "POST";

            string content = string.Join("&", values.Select(v => v.Key + "=" + v.Value));
            byte[] byteContent = Encoding.UTF8.GetBytes(content);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteContent.Length;
            var dataStream = request.GetRequestStream();
            dataStream.Write(byteContent, 0, byteContent.Length);
            dataStream.Close();
        }

        private static string GetCreditCardKindCode(CreditCardKind kind)
        {
            switch (kind)
            {
                case CreditCardKind.Visa:
                    return "VI";
                case CreditCardKind.MasterCard:
                    return "MC";
                case CreditCardKind.Discover:
                    return "DI";
                case CreditCardKind.AmericanExpress:
                    return "AX";
                default:
                    throw new NotSupportedException(string.Format("The credit card kind {0} is not supported", kind));
            }
        }

        private static string GetCurrencyCode(Currency currency)
        {
            return currency.ToString().ToUpperInvariant();
        }

        private static string GetBillingFrequencyCode(BillingFrequency frequency)
        {
            return frequency.ToString().ToUpperInvariant();
        }
    }
}
