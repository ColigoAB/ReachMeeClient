using Coligo.ReachMee.Data.ApiClients;
using Coligo.ReachMee.Data.Context;
using Coligo.ReachMee.Data.Exceptions;
using Coligo.ReachMee.Data.Interfaces;
using Coligo.ReachMee.Data.Models;
using Coligo.ReachMee.Data.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coligo.ReachMee.Tests
{
    [TestClass]
    public class RoleTests
    {
        ReachMeeService _service;
        static ReachMeeContextInMemory _context = new ReachMeeContextInMemory();
        readonly IApiClient _reachMeeClient = new ReachMeeClientInMemory(_context);

        #region Init and Cleanup
        [TestInitialize]
        public void TestInitialize()
        {
            _context.AddRole(new Role()
            {
                Role_id = "MBx2mag9Dc",
                Description = "Role1"
            });
            _context.AddRole(new Role()
            {
                Role_id = "4g7rqged0t",
                Description = "Role2"
            });

            _context.AddUser(new User()
            {
                First_name = "TestUser",
                Surname = "Surname",
                Employee_number = "1",
                User_name = "User_1"
            });

            _context.AddOrganization(new Organization()
            {
                Name = "Org1",
                External_org_unit_id = "ExtOrgId1"
            });
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _context = new ReachMeeContextInMemory();
        }
        #endregion
        [TestMethod]
        public void GetRoles_Success()
        {
            //Arrange
            _service = new ReachMeeService(_reachMeeClient);

            //Act
            List<Role> result = _service.GetRoles();

            //Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void Assign_Role_To_User_Success()
        {
            //Arrange
            _service = new ReachMeeService(_reachMeeClient);

            //Act
            _service.AssignRole("MBx2mag9Dc", 1, "ExtOrgId1");

            //Assert
        }

        [TestMethod]
        public void Assign_Role_To_User_Fails_UserNotFound()
        {
            //Arrange
            _service = new ReachMeeService(_reachMeeClient);

            //Act
            //Assert
            Assert.ThrowsException<UserNotFoundException>(() => _service.AssignRole("MBx2mag9Dc", 2, "ExtOrgId1"));
        }
    }
}
