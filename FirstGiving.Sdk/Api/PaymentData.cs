using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace FirstGiving.Sdk.Api
{
    /// <summary>
    ///     Basic payment data
    /// </summary>
    public abstract class PaymentData
    {
        private string title;
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get { return this.title; }
            set
            {
                Validate.Is.LowerThanOrEqualTo((value ?? string.Empty).Length, 10, "Title.Length");
                this.title = value;
            }
        }

        private string firstName;
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName
        {
            get { return this.firstName; }
            set
            {
                Validate.Is.NotNullOrWhiteSpace(value, "FirstName");
                Validate.Is.LowerThanOrEqualTo(value.Length, 100, "FirstName.Length");
                this.firstName = value;
            }
        }

        private string middleName;
        /// <summary>
        /// Gets or sets the name of the middle.
        /// </summary>
        /// <value>
        /// The name of the middle.
        /// </value>
        public string MiddleName
        {
            get { return this.middleName; }
            set
            {
                Validate.Is.LowerThanOrEqualTo((value ?? string.Empty).Length, 100, "MiddleName.Length");
                this.middleName = value;
            }
        }

        private string lastName;
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName
        {
            get { return this.lastName; }
            set
            {
                Validate.Is.NotNullOrWhiteSpace(value, "LastName");
                Validate.Is.LowerThanOrEqualTo(value.Length, 100, "LastName.Length");
                this.lastName = value;
            }
        }

        private string address1;
        /// <summary>
        /// Gets or sets the address1.
        /// </summary>
        /// <value>
        /// The address1.
        /// </value>
        public string Address1
        {
            get { return this.address1; }
            set
            {
                Validate.Is.NotNullOrWhiteSpace(value, "Address1");
                Validate.Is.LowerThanOrEqualTo(value.Length, 255, "Address1.Length");
                this.address1 = value;
            }
        }

        private string address2;
        /// <summary>
        /// Gets or sets the address2.
        /// </summary>
        /// <value>
        /// The address2.
        /// </value>
        public string Address2
        {
            get { return this.address2; }
            set
            {
                Validate.Is.LowerThanOrEqualTo((value ?? string.Empty).Length, 255, "Address2.Length");
                this.address2 = value;
            }
        }

        private string address3;
        /// <summary>
        /// Gets or sets the address3.
        /// </summary>
        /// <value>
        /// The address3.
        /// </value>
        public string Address3
        {
            get { return this.address3; }
            set
            {
                Validate.Is.LowerThanOrEqualTo((value ?? string.Empty).Length, 255, "Address3.Length");
                this.address3 = value;
            }
        }

        private string city;
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public string City
        {
            get { return this.city; }
            set
            {
                Validate.Is.NotNullOrWhiteSpace(value, "City");
                Validate.Is.LowerThanOrEqualTo(value.Length, 35, "City.Length");
                this.city = value;
            }
        }

        private string state;
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public string State
        {
            get { return this.state; }
            set
            {
                Validate.Is.NotNullOrWhiteSpace(value, "State");
                Validate.Is.LowerThanOrEqualTo((value ?? string.Empty).Length, 30, "State.Length");
                this.state = value;
            }
        }

        private string zipCode;
        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>
        /// The zip code.
        /// </value>
        public string ZipCode
        {
            get { return this.zipCode; }
            set
            {
                Validate.Is.NotNullOrWhiteSpace(value, "ZipCode");
                Validate.Is.LowerThanOrEqualTo(value.Length, 20, "ZipCode.Length");
                this.zipCode = value;
            }
        }

        private string country;
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public string Country
        {
            get { return this.country; }
            set
            {
                Validate.Is.NotNullOrWhiteSpace(value, "Country");
                Validate.Is.EqualTo(value.Length, 2, "Country.Length");
                this.country = value;
            }
        }

        private MailAddress emailAddress;
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public MailAddress EmailAddress
        {
            get { return this.emailAddress; }
            set
            {
                Validate.Is.NotNull(value, "EmailAddress");
                Validate.Is.LowerThanOrEqualTo(value.Address.Length, 100, "EmailAddress.Length");
                this.emailAddress = value;
            }
        }

        private string phoneNumber;
        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string PhoneNumber
        {
            get { return this.phoneNumber; }
            set
            {
                Validate.Is.LowerThanOrEqualTo((value ?? string.Empty).Length, 20, "PhoneNumber.Length");
                this.phoneNumber = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentData"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="address1">The address1.</param>
        /// <param name="city">The city.</param>
        /// <param name="zipCode">The zip code.</param>
        /// <param name="country">The country.</param>
        /// <param name="emailAddress">The email address.</param>
        protected PaymentData(string firstName, string lastName, string address1, string city, string zipCode, string country, MailAddress emailAddress)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Address1 = address1;
            this.City = city;
            this.ZipCode = zipCode;
            this.Country = country;
            this.EmailAddress = emailAddress;
        }
    }
}
