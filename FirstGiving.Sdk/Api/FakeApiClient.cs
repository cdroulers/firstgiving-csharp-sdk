using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FirstGiving.Sdk.Api
{
    /// <summary>
    ///     An API client that doesn't do anything.
    /// </summary>
    public class FakeApiClient : IApiClient
    {
        private readonly IApiClient decorated;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeApiClient"/> class.
        /// </summary>
        /// <param name="decorated">The decorated.</param>
        public FakeApiClient(IApiClient decorated)
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
        /// Make a donation via a credit card.
        /// </summary>
        /// <param name="donation">The donation.</param>
        /// <param name="paymentData">The payment data.</param>
        /// <param name="remoteAddress">The remote address.</param>
        /// <returns></returns>
        public string DonateByCreditCard(Donation donation, CreditCardPaymentData paymentData, System.Net.IPAddress remoteAddress)
        {
            Thread.Sleep(2500);
            return Guid.NewGuid().ToString().Replace("-", string.Empty);
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
        public string DonateByCreditCardRecurring(Donation donation, CreditCardPaymentData paymentData, System.Net.IPAddress remoteAddress, BillingFrequency frequency, int? term)
        {
            Thread.Sleep(2500);
            return Guid.NewGuid().ToString().Replace("-", string.Empty);
        }
    }
}
