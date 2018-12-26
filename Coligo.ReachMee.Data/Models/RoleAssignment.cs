using Coligo.ReachMee.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coligo.ReachMee.Data.Models
{
    public class RoleAssignment : IRoleAssignment
    {
        public string Role_id { get; set; }
        public string Role_name { get; set; }
        public string description { get; set; }
        public Org_unit Org_unit { get; set; }
    }
}
