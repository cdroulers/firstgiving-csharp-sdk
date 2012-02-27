using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace FirstGiving.Sdk.Api
{
    /// <summary>
    ///     A test api client that always uses test data.
    /// </summary>
    public class TestApiClient : IApiClient
    {
        private readonly IApiClient decorated;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestApiClient"/> class.
        /// </summary>
        /// <param name="decorated">The decorated.</param>
        public TestApiClient(IApiClient decorated)
        {
            this.decorated = decorated;
        }

        /// <summary>
        /// Gets the application key.
        /// </summary>
        public string ApplicationKey
        {
            get { return this.decorated.ApplicationKey; }
        }

        /// <summary>
        /// Gets the security token.
        /// </summary>
        public string SecurityToken
        {
            get { return this.decorated.SecurityToken; }
        }

        /// <summary>
        /// Gets the API endpoint.
        /// </summary>
        public Uri ApiEndpoint
        {
            get { return this.decorated.ApiEndpoint; }
        }

        /// <summary>
        /// Edits the data so that it uses test data always.
        /// </summary>
        /// <param name="paymentData">The payment data.</param>
        private void EditData(CreditCardPaymentData paymentData)
        {
            paymentData.Address1 = "1 Main St.";
            paymentData.City = "Burlington";
            paymentData.ZipCode = "01803";
            paymentData.State = "MA";
            paymentData.Country = "US";
            paymentData.UpdateCardData("4457010000000009", CreditCardKind.Visa, new DateTime(2025, 01, 01), "111");
        }

        /// <summary>
        /// Make a donation via a credit card.
        /// </summary>
        /// <param name="donation">The donation.</param>
        /// <param name="paymentData">The payment data.</param>
        /// <param name="remoteAddress">The remote address.</param>
        /// <returns></returns>
        public string DonateByCreditCard(Donation donation, CreditCardPaymentData paymentData, IPAddress remoteAddress)
        {
            EditData(paymentData);
            return this.decorated.DonateByCreditCard(donation, paymentData, remoteAddress);
        }

        /// <summary>
        /// Make a recurring donation via credit card.
        /// </summary>
        /// <param name="donation">The donation.</param>
        /// <param name="paymentData">The payment data.</param>
        /// <param name="remoteAddress">The remote address.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public string DonateByCreditCardRecurring(Donation donation, CreditCardPaymentData paymentData, IPAddress remoteAddress, BillingFrequency frequency, int? term)
        {
            EditData(paymentData);
            return this.decorated.DonateByCreditCardRecurring(donation, paymentData, remoteAddress, frequency, term);
        }
    }
}
