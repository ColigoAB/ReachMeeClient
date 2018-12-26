using Coligo.ReachMee.Data.Models;

namespace Coligo.ReachMee.Data.Interfaces
{
    interface IRoleAssignment
    {
        string Role_id { get; set; }
        string Role_name { get; set; }
        string description { get; set; }
        Org_unit Org_unit { get; set; }
    }
}
