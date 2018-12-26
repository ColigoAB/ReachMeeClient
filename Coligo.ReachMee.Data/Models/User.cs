using Coligo.ReachMee.Data.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Coligo.ReachMee.Data.Models
{
    /// <summary>
    /// User object in ReachMee
    /// </summary>
    public class User : IUser
    {
        public User() { }

        public User(IUser user)
        {
            User_name = user.User_name;
            User_id = user.User_id;
            Surname = user.Surname;
            First_name = user.First_name;
            Date_removed = user.Date_removed;
            Last_login = user.Last_login;
            Email_home = user.Email_home;
            Org_unit_id = user.Org_unit_id;
            Email_work = user.Email_work;
            External_org_unit_id = user.External_org_unit_id;
            Employee_number = user.Employee_number;
            Telephone_1 = user.Telephone_1;
            Telephone_2 = user.Telephone_2;
            Telephone_3 = user.Telephone_3;
            Login_type = user.Login_type;
            Password = user.Password;
        }

        /// <summary>
        /// the user name of the user
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string User_name { get; set; }
        /// <summary>
        /// the last name of the user
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Surname { get; set; }
        /// <summary>
        /// the first name of the user
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string First_name { get; set; }
        /// <summary>
        /// date this use was inactivated (optinal)
        /// </summary>
        public string Date_removed { get; set; }
        /// <summary>
        /// the active/inactive status of the user (optional)
        /// </summary>
        public string Last_login { get; set; }
        /// <summary>
        /// the home email of the user (optional)
        /// </summary>
        public string Email_home { get; set; }
        /// <summary>
        /// ReachMee's internal organization unit id where the user belongs (optional)
        /// </summary>
        public string Org_unit_id { get; set; }
        /// <summary>
        /// the work email of the user (optional)
        /// </summary>
        public string Email_work { get; set; }
        /// <summary>
        /// external organization unit id where the user belongs (optional)
        /// </summary>
        public string External_org_unit_id { get; set; }
        /// <summary>
        /// the employee number of the user
        /// </summary>
        public string Employee_number { get; set; }
        /// <summary>
        /// the id of the user (optional)
        /// </summary>
        public int User_id { get; set; }
        /// <summary>
        /// the telephone 1 of the user (optional)
        /// </summary>
        public string Telephone_1 { get; set; }
        /// <summary>
        /// the telephone 2 of the user (optional)
        /// </summary>
        public string Telephone_2 { get; set; }
        /// <summary>
        /// the telephone 3 of the user (optional)
        /// </summary>
        public string Telephone_3 { get; set; }
        /// <summary>
        /// 'INT' or 'EXT', EXT means SSO user
        /// </summary>
        public string Login_type { get; set; }
        /// <summary>
        ///  the password for the user (optional)
        /// </summary>
        public string Password { get; set; }
    }
}
