using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstGiving.Sdk.Charities
{
    /// <summary>
    ///     A charity from the FirstGiving API
    /// </summary>
    public class Charity
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public readonly Guid UUID;
        /// <summary>
        /// Name
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// Alias of the non-profit
        /// </summary>
        public readonly string Alias;
        /// <summary>
        /// Government unique identifier
        /// </summary>
        public readonly string GovernmentID;
        /// <summary>
        /// First address line
        /// </summary>
        public readonly string Address1;
        /// <summary>
        /// Second address line
        /// </summary>
        public readonly string Address2;
        /// <summary>
        /// Third address line
        /// </summary>
        public readonly string Address3;
        /// <summary>
        /// Address lines
        /// </summary>
        public readonly string AddressLines;
        /// <summary>
        /// Full address
        /// </summary>
        public readonly string Address;
        /// <summary>
        /// City
        /// </summary>
        public readonly string City;
        /// <summary>
        /// Region
        /// </summary>
        public readonly string Region;
        /// <summary>
        /// Postal code
        /// </summary>
        public readonly string PostalCode;
        /// <summary>
        /// County
        /// </summary>
        public readonly string County;
        /// <summary>
        /// Country
        /// </summary>
        public readonly string Country;
        /// <summary>
        /// Phone number
        /// </summary>
        public readonly string PhoneNumber;
        /// <summary>
        /// Area code
        /// </summary>
        public readonly string AreaCode;
        /// <summary>
        /// URL if known
        /// </summary>
        public readonly Uri Url;
        /// <summary>
        /// Government category code
        /// </summary>
        public readonly string CategoryCode;
        /// <summary>
        /// Latitude of the address
        /// </summary>
        public readonly double Latitude;
        /// <summary>
        /// Longitude of the address
        /// </summary>
        public readonly double Longitude;
        /// <summary>
        /// If the non-profit status was revoked.
        /// </summary>
        public readonly bool Revoked;

        /// <summary>
        /// Initializes a new instance of the <see cref="Charity"/> class.
        /// </summary>
        /// <param name="uuid">The UUID.</param>
        /// <param name="name">The name.</param>
        /// <param name="alias">The alias.</param>
        /// <param name="governmentID">The government ID.</param>
        /// <param name="address1">The address1.</param>
        /// <param name="address2">The address2.</param>
        /// <param name="address3">The address3.</param>
        /// <param name="addressLines">The address lines.</param>
        /// <param name="address">The address.</param>
        /// <param name="city">The city.</param>
        /// <param name="region">The region.</param>
        /// <param name="postalCode">The postal code.</param>
        /// <param name="county">The county.</param>
        /// <param name="country">The country.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="areaCode">The area code.</param>
        /// <param name="url">The URL.</param>
        /// <param name="categoryCode">The category code.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="revoked">if set to <c>true</c> [revoked].</param>
        public Charity(Guid uuid, string name, string @alias, string governmentID, string address1, string address2, string address3, string addressLines, string address, string city, string region, string postalCode, string county, string country, string phoneNumber, string areaCode, Uri url, string categoryCode, double latitude, double longitude, bool revoked)
        {
            this.UUID = uuid;
            this.Name = name;
            this.Alias = alias;
            this.GovernmentID = governmentID;
            this.Address1 = address1;
            this.Address2 = address2;
            this.Address3 = address3;
            this.AddressLines = addressLines;
            this.Address = address;
            this.City = city;
            this.Region = region;
            this.PostalCode = postalCode;
            this.County = county;
            this.Country = country;
            this.PhoneNumber = phoneNumber;
            this.AreaCode = areaCode;
            this.Url = url;
            this.CategoryCode = categoryCode;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Revoked = revoked;
        }
    }
}
