using System.Collections.Generic;

namespace Coligo.ReachMee.Data.Interfaces
{
    public interface IOrganization
    {
        int Org_unit_id { get; set; }
        string Review_person_email { get; set; }
        string Postal_county { get; set; }
        string Name { get; set; }
        string Postal_address { get; set; }
        string Visit_address { get; set; }
        string External_org_unit_id { get; set; }
        string Visit_city { get; set; }
        string Postal_city { get; set; }
        string Visit_zipcode { get; set; }
        string Contact_email { get; set; }
        string Parent_external_org_unit_id { get; set; }
        string Postal_companyname { get; set; }
        string Visit_country { get; set; }
        string Postal_country { get; set; }
        string Vat_number { get; set; }
        string Contact_name { get; set; }
        string Contact_phone { get; set; }
        string Postal_zipcode { get; set; }
        string Visit_county { get; set; }
        ICollection<IOrganization> Children { get; set; }
    }
}
