using Coligo.ReachMee.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coligo.ReachMee.Data.Models
{
    public class Role : IRole
    {
        public Role() { }

        public Role(IRole role)
        {
            Name = role.Name;
            Role_id = role.Role_id;
            Description = role.Description;
            External_id = role.External_id;
        }

        public string Name { get; set; }
        public string Role_id { get; set; }
        public string Description { get; set; }
        public string External_id { get; set; }
    }
}
