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
    /// <summary>
    ///     A REST client for the FirstGiving API
    /// </summary>
    public class ApiClient : BaseRestClient, IApiClient
    {
        /// <summary>
        /// Gets the application key.
        /// </summary>
        public string ApplicationKey { get; private set; }
        /// <summary>
        /// Gets the security token.
        /// </summary>
        public string SecurityToken { get; private set; }

        /// <summary>
        /// Gets the lastest response.
        /// </summary>
        public ResponseData LastestResponse { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient"/> class.
        /// </summary>
        /// <param name="applicationKey">The application key.</param>
        /// <param name="securityToken">The security token.</param>
        /// <param name="apiEndpoint">The API endpoint.</param>
        public ApiClient(string applicationKey, string securityToken, Uri apiEndpoint)
            : base(apiEndpoint)
        {
            this.ApplicationKey = applicationKey;
            this.SecurityToken = securityToken;
        }

        /// <summary>
        /// Says the hello.
        /// </summary>
        /// <returns></returns>
        public string SayHello()
        {
            var xml = this.SendApiRequest("/default/test/0", "GET");

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

            var xml = this.SendApiRequest("/donation/creditcard", "POST", parameters);

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

            var xml = this.SendApiRequest("/donation/recurringcreditcardprofile", "POST", parameters);

            var transactionID = xml.Descendants("recurringDonationProfileId").First();
            return transactionID.Value;
        }

        /// <summary>
        /// Gets the credit card recurring profile with the specified ID.
        /// </summary>
        /// <param name="profileID">The profile ID.</param>
        /// <returns></returns>
        public string GetCreditCardRecurringProfile(string profileID)
        {
            var parameters = new Dictionary<string, string>()
            {
                { "id", profileID }
            };
            var xml = this.SendApiRequest("/recurring/billingprofile", "GET", parameters);

            string temp = xml.ToString();
            return temp;
        }

        /// <summary>
        /// Deletes the credit card recurring profile with the specified ID.
        /// </summary>
        /// <param name="profileID">The profile ID.</param>
        public void DeleteCreditCardRecurringProfile(string profileID)
        {
            var parameters = new Dictionary<string, string>()
            {
                { "id", profileID }
            };
            var xml = this.SendApiRequest("/recurring/billingprofile", "DELETE", parameters);

            string temp = xml.ToString();
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

            var xml = this.SendApiRequest("/verify", "POST", parameters);
            var valid = xml.Descendants("valid").First();
            return valid.Value == "1";
        }

        /// <summary>
        /// Called just before a request is made.
        /// </summary>
        /// <param name="request">The request.</param>
        protected override void PreRequest(HttpWebRequest request)
        {
            request.Headers["JG_APPLICATIONKEY"] = this.ApplicationKey;
            request.Headers["JG_SECURITYTOKEN"] = this.SecurityToken;
        }

        /// <summary>
        /// Called after a request is made.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="resultBody">The result body.</param>
        /// <param name="response">The response.</param>
        protected override void PostRequest(string result, XDocument resultBody, HttpWebResponse response)
        {
            this.LastestResponse = new ResponseData(resultBody, result, response.Headers["Jg-Response-Signature"]);
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
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedException(this.ApplicationKey, this.SecurityToken, e, resultBody);
                case HttpStatusCode.BadRequest:
                    string errorTarget;
                    throw new InvalidInputException(GetInvalidInputErrorMessage(resultBody, out errorTarget), e, resultBody, errorTarget);
                case HttpStatusCode.InternalServerError:
                    throw new ServerErrorException(GetServerErrorErrorMessage(resultBody), e, resultBody);
            }
        }

        private static string GetInvalidInputErrorMessage(XDocument responseContent, out string errorTarget)
        {
            var node = responseContent.Descendants("firstGivingResponse").First();
            string errorMessage = node.Attributes("verboseErrorMessage").First().Value;
            var target = node.Attributes("errorTarget").FirstOrDefault();
            errorTarget = target == null ? string.Empty : target.Value;
            return string.Format(@"An error occurred for the target ""{0}"". Error message was:
{1}", errorTarget, errorMessage);
        }

        private static string GetServerErrorErrorMessage(XDocument responseContent)
        {
            var node = responseContent.Descendants("firstGivingResponse").First();
            return "Server error. Message was:" + Environment.NewLine + node.Attributes("verboseErrorMessage").First().Value;
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
