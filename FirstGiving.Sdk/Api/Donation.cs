using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstGiving.Sdk.Api
{
    /// <summary>
    ///     A FirstGiving donation.
    /// </summary>
    public class Donation
    {
        /// <summary>
        /// Gets or sets the charity GUID. A UUID identifier provided by FirstGiving which identifies the recipient of the donation.
        /// </summary>
        public Guid CharityID { get; set; }
        /// <summary>
        /// Gets or sets the event ID. An identifier provided by FirstGiving which identifies the event associated with the donation.
        /// </summary>
        public string EventID { get; set; }
        /// <summary>
        /// Gets or sets the fundraiser ID. A universally unique ID which identifies the user account that was responsible for the donation that was collected. This is almost always a different person than the donor.
        /// </summary>
        public Guid? FundraiserID { get; set; }
        /// <summary>
        /// Gets or sets the order ID. A universally unique ID genereated by the 3rd party which identifies the donation in their system.
        /// </summary>
        public Guid? OrderID { get; set; }

        private string description;
        /// <summary>
        /// Gets or sets the description. A short textual description of the donation.
        /// </summary>
        public string Description
        {
            get { return this.description; }
            set
            {
                Validate.Is.Not.NullOrWhiteSpace(value, "Description");
                this.description = value;
            }
        }
        /// <summary>
        /// Gets or sets whether to report donation to tax authority. Indicates whether or not this donation should be reported to the tax authority in the donor’s country. *Not applicable in the U.S.
        /// </summary>
        public bool ReportDonationToTaxAuthority { get; set; }
        /// <summary>
        /// Gets or sets the the national id number assigned to the donor who wants the donation reported to the tax authority. If the customer does not want the donation reported, just pass a blank string here.  *Not applicable in the U.S.
        /// </summary>
        public string PersonalIdentificationNumber { get; set; }
        /// <summary>
        /// Gets or sets the message from donor to the charity.
        /// </summary>
        public string DonationMessage { get; set; }
        /// <summary>
        /// Gets or sets the name of individual or organization that the donation was made to honor.
        /// </summary>
        public string HonorMemoryName { get; set; }
        private decimal amount;
        /// <summary>
        /// Gets or sets the amount of the donation.
        /// </summary>
        public decimal Amount
        {
            get { return this.amount; }
            set
            {
                Validate.Is.HigherThanOrEqualTo(value, 5.0M, "Amount");
                this.amount = value;
            }
        }
        /// <summary>
        /// Gets or sets the currency
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Donation"/> class.
        /// </summary>
        /// <param name="charityID">The charity ID.</param>
        /// <param name="description">The description.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        public Donation(Guid charityID, string description, decimal amount, Currency currency = Currency.USD)
        {
            this.CharityID = charityID;
            this.Description = description;
            this.Amount = amount;
            this.Currency = currency;
        }
    }
}
