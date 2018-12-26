using Coligo.ReachMee.Data.Interfaces;
using System.Collections.Generic;

namespace Coligo.ReachMee.Data.Models
{
    /// <summary>
    /// Organization Unit in ReachMee
    /// </summary>
    public class Organization : IOrganization
    {
        public Organization() { }
        public Organization(IOrganization organization)
        {
            Org_unit_id = organization.Org_unit_id;
            Review_person_email = organization.Review_person_email;
            Postal_county = organization.Postal_county;
            Name = organization.Name;
            Postal_address = organization.Postal_address;
            Visit_address = organization.Visit_address;
            External_org_unit_id = organization.External_org_unit_id;
            Visit_city = organization.Visit_city;
            Postal_city = organization.Postal_city;
            Visit_zipcode = organization.Visit_zipcode;
            Contact_email = organization.Contact_email;
            Parent_external_org_unit_id = organization.Parent_external_org_unit_id;
            Postal_companyname = organization.Postal_companyname;
            Visit_country = organization.Visit_country;
            Postal_country = organization.Postal_country;
            Vat_number = organization.Vat_number;
            Contact_name = organization.Contact_name;
            Contact_phone = organization.Contact_phone;
            Postal_zipcode = organization.Postal_zipcode;
            Visit_county = organization.Visit_county;
            Children = organization.Children;
        }

        public int Org_unit_id { get; set; }
        public string Review_person_email { get; set; }
        public string Postal_county { get; set; }
        public string Name { get; set; }
        public string Postal_address { get; set; }
        public string Visit_address { get; set; }
        public string External_org_unit_id { get; set; }
        public string Visit_city { get; set; }
        public string Postal_city { get; set; }
        public string Visit_zipcode { get; set; }
        public string Contact_email { get; set; }
        public string Parent_external_org_unit_id { get; set; }
        public string Postal_companyname { get; set; }
        public string Visit_country { get; set; }
        public string Postal_country { get; set; }
        public string Vat_number { get; set; }
        public string Contact_name { get; set; }
        public string Contact_phone { get; set; }
        public string Postal_zipcode { get; set; }
        public string Visit_county { get; set; }
        public ICollection<IOrganization> Children { get; set; }
    }
}
