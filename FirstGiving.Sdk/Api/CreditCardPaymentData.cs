using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace FirstGiving.Sdk.Api
{
    /// <summary>
    ///     Payment data for a Credit card
    /// </summary>
    public class CreditCardPaymentData : PaymentData
    {
        /// <summary>
        /// Gets the card number.
        /// </summary>
        public string CardNumber { get; private set; }

        /// <summary>
        /// Gets the kind of the card.
        /// </summary>
        /// <value>
        /// The kind of the card.
        /// </value>
        public CreditCardKind CardKind { get; private set; }

        /// <summary>
        /// Gets the card expiration date.
        /// </summary>
        public DateTime CardExpirationDate { get; private set; }

        /// <summary>
        /// Gets the card validation number.
        /// </summary>
        public string CardValidationNumber { get; private set; }

        /// <summary>
        /// Gets or sets the billing descriptor.
        /// </summary>
        /// <value>
        /// The billing descriptor, which appears on the credit card statement.
        /// </value>
        public string BillingDescriptor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditCardPaymentData"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="address1">The address1.</param>
        /// <param name="city">The city.</param>
        /// <param name="zipCode">The zip code.</param>
        /// <param name="country">The country.</param>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="cardNumber">The card number.</param>
        /// <param name="cardKind">Kind of the card.</param>
        /// <param name="cardExpirationDate">The card expiration date.</param>
        /// <param name="cardValidationNumber">The card validation number.</param>
        public CreditCardPaymentData(string firstName, string lastName, string address1, string city, string zipCode, string country, MailAddress emailAddress,
            string cardNumber, CreditCardKind cardKind, DateTime cardExpirationDate, string cardValidationNumber)
            : base(firstName, lastName, address1, city, zipCode, country, emailAddress)
        {
            this.UpdateCardData(cardNumber, cardKind, cardExpirationDate, cardValidationNumber);
        }

        /// <summary>
        /// Updates the card data.
        /// </summary>
        /// <param name="cardNumber">The card number.</param>
        /// <param name="cardKind">Kind of the card.</param>
        /// <param name="cardExpirationDate">The card expiration date.</param>
        /// <param name="cardValidationNumber">The card validation number.</param>
        public void UpdateCardData(string cardNumber, CreditCardKind cardKind, DateTime cardExpirationDate, string cardValidationNumber)
        {
            Validate.Is.Not.NullOrWhiteSpace(cardNumber, "CardNumber");
            switch (cardKind)
            {
                case CreditCardKind.AmericanExpress:
                    Validate.Is.EqualTo(cardNumber.Length, 15, "CardNumber.AmericanExpress.Length");
                    break;
                default:
                    Validate.Is.EqualTo(cardNumber.Length, 16, "CardNumber.Length");
                    break;
            }

            Validate.Is.Not.NullOrWhiteSpace(cardValidationNumber, "CardValidationNumber");
            switch (cardKind)
            {
                case CreditCardKind.Visa:
                    Validate.Is.EqualTo(cardValidationNumber.Length, 3, "CardValidationNumber.Visa.CVV2.Length");
                    break;
                case CreditCardKind.MasterCard:
                    Validate.Is.EqualTo(cardValidationNumber.Length, 3, "CardValidationNumber.Visa.CVC2.Length");
                    break;
                case CreditCardKind.AmericanExpress:
                    Validate.Is.EqualTo(cardValidationNumber.Length, 4, "CardValidationNumber.Visa.CID.Length");
                    break;
            }

            this.CardNumber = cardNumber;
            this.CardKind = cardKind;
            this.CardExpirationDate = cardExpirationDate;
            this.CardValidationNumber = cardValidationNumber;
        }
    }
}
