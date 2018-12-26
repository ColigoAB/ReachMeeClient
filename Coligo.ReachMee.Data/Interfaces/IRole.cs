using System;
using System.Collections.Generic;
using System.Text;

namespace Coligo.ReachMee.Data.Interfaces
{
    public interface IRole
    {
        string Name { get; set; }
        string Role_id { get; set; }
        string Description { get; set; }
        string External_id { get; set; }
    }
}
