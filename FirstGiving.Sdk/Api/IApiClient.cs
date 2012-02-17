using System;
using System.Net;

namespace FirstGiving.Sdk.Api
{
    /// <summary>
    ///     An interface describing available methods for the FirstGiving API
    /// </summary>
    public interface IApiClient
    {
        /// <summary>
        /// Gets the application key.
        /// </summary>
        string ApplicationKey { get; }
        /// <summary>
        /// Gets the security token.
        /// </summary>
        string SecurityToken { get; }
        /// <summary>
        /// Gets the API endpoint.
        /// </summary>
        Uri ApiEndpoint { get; }
        /// <summary>
        /// Make a donation via a credit card.
        /// </summary>
        /// <param name="donation">The donation.</param>
        /// <param name="paymentData">The payment data.</param>
        /// <param name="remoteAddress">The remote address.</param>
        /// <returns></returns>
        string DonateByCreditCard(Donation donation, CreditCardPaymentData paymentData, IPAddress remoteAddress);
        /// <summary>
        /// Make a recurring donation via credit card.
        /// </summary>
        /// <param name="donation">The donation.</param>
        /// <param name="paymentData">The payment data.</param>
        /// <param name="remoteAddress">The remote address.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        string DonateByCreditCardRecurring(Donation donation, CreditCardPaymentData paymentData, IPAddress remoteAddress, BillingFrequency frequency, int? term);
    }
}