using Coligo.ReachMee.Data.Models;
using System.Collections.Generic;

namespace Coligo.ReachMee.Data.Interfaces
{
    public interface IReachMeeContext
    {
        int AddUser(IUser user);
        void AddOrganization(IOrganization organization);
        void AddRole(IRole role);
        void AddRoleAssignment(int user_id, string role_id, string external_org_unit_id);
        List<User> GetUsers();
        List<Role> GetRoles();
        List<Organization> GetOrganizations();
        List<Organization> GetOrganizations(string query);
        List<Organization> GetOrganizations(int query);
    }
}
