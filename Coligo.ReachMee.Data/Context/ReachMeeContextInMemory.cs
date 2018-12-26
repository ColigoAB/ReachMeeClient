using Coligo.ReachMee.Data.Exceptions;
using Coligo.ReachMee.Data.Interfaces;
using Coligo.ReachMee.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coligo.ReachMee.Data.Context
{
    public class ReachMeeContextInMemory : IReachMeeContext
    {
        private int _nextUserId = 1;
        private int _nextOrgId = 1;
        private int _nextRoleAssignmentId = 1;
        private readonly Dictionary<int, User> _users = new Dictionary<int, User>();
        private readonly Dictionary<string, Organization> _orgs = new Dictionary<string, Organization>();
        private readonly Dictionary<string, Role> _roles = new Dictionary<string, Role>();
        private readonly Dictionary<int, RoleAssignment> _roleAssignments = new Dictionary<int, RoleAssignment>();

        public List<Role> GetRoles()
        {
            return _roles.Values.ToList();
        }
        public List<User> GetUsers()
        {
            return _users.Values.ToList();
        }
        public List<Organization> GetOrganizations()
        {
            return _orgs.Values.ToList();
        }
        public List<Organization> GetOrganizations(string query)
        {
            return _orgs.Values.Where(x => x.External_org_unit_id == query).ToList();
        }
        public List<Organization> GetOrganizations(int query)
        {
            return _orgs.Values.Where(x => x.Org_unit_id == query).ToList();
        }
        public void AddRoleAssignment(int user_id, string role, string external_org_unit_id)
        {

        }
        public void AddOrganization(IOrganization organization)
        {
            if (_orgs.Values.FirstOrDefault(x => x.External_org_unit_id == organization.External_org_unit_id && !String.IsNullOrWhiteSpace(organization.External_org_unit_id)) != null)
                throw new Exception(); //TODO: Due to a known bug in RechMee API, this returns 500 instead of 409

            _orgs.Add(_nextOrgId.ToString(), new Organization(organization)
            {
                Org_unit_id = _nextOrgId
            });
            _nextOrgId++;
        }
        public void AddRole(IRole role)
        {
            _roles.Add(role.Role_id, new Role(role));
        }

        public int AddUser(IUser user)
        {
            if (_users.Values.FirstOrDefault(x => x.Employee_number == user.Employee_number) != null)
                throw new UserAlreadyExistsException();
            if (_users.Values.FirstOrDefault(x => x.User_name == user.User_name) != null)
                throw new UserAlreadyExistsException();

            _users.Add(_nextUserId, new User(user)
            {
                User_id = _nextUserId
            });
            return _nextUserId++;
        }
    }
}
