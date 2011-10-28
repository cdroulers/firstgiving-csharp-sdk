using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstGiving.Sdk.Charities
{
    public class Charity
    {
        public readonly Guid UUID;
        public readonly string Name;
        public readonly string Alias;
        public readonly string GovernmentID;
        public readonly string Address1;
        public readonly string Address2;
        public readonly string Address3;
        public readonly string AddressLines;
        public readonly string Address;
        public readonly string City;
        public readonly string Region;
        public readonly string PostalCode;
        public readonly string County;
        public readonly string Country;
        public readonly string PhoneNumber;
        public readonly string AreaCode;
        public readonly Uri Url;
        public readonly string CategoryCode;
        public readonly double Latitude;
        public readonly double Longitude;
        public readonly bool Revoked;

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
