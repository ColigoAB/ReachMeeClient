namespace Coligo.ReachMee.Data.Interfaces
{
    public interface IUser
    {
        string User_name { get; set; }
        string Surname { get; set; }
        string First_name { get; set; }
        string Date_removed { get; set; }
        string Last_login { get; set; }
        string Email_home { get; set; }
        string Org_unit_id { get; set; }
        string Email_work { get; set; }
        string External_org_unit_id { get; set; }
        string Employee_number { get; set; }
        int User_id { get; set; }
        string Telephone_1 { get; set; }
        string Telephone_2 { get; set; }
        string Telephone_3 { get; set; }
        string Login_type { get; set; }
        string Password { get; set; }
    }
}
