using Coligo.ReachMee.Data.ApiClients;
using Coligo.ReachMee.Data.Context;
using Coligo.ReachMee.Data.Exceptions;
using Coligo.ReachMee.Data.Interfaces;
using Coligo.ReachMee.Data.Models;
using Coligo.ReachMee.Data.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Coligo.ReachMee.Tests
{
    [TestClass]
    public class OrganizationTests
    {
        ReachMeeService _service;
        static ReachMeeContextInMemory _context = new ReachMeeContextInMemory();
        readonly IApiClient _reachMeeClient = new ReachMeeClientInMemory(_context);

        #region Init and Cleanup
        [TestInitialize]
        public void TestInitialize()
        {
            _context.AddOrganization(new Organization()
            {
                Name = "Org1",
                External_org_unit_id = "1"
            });
            _context.AddOrganization(new Organization()
            {
                Name = "Org2",
                External_org_unit_id = "2"
            });
            _context.AddOrganization(new Organization()
            {
                Name = "Org3",
                External_org_unit_id = "3"
            });
            _context.AddOrganization(new Organization()
            {
                Name = "Org4",
                External_org_unit_id = "4"
            });
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _context = new ReachMeeContextInMemory();
        }
        #endregion

        #region Create Tests
        [TestMethod]
        public void CreateOrg_Success()
        {
            //Arrange
            _service = new ReachMeeService(_reachMeeClient);
            var org5 = new Organization()
            {
                Name = "Org5",
                External_org_unit_id = "5"
            };
            var org6 = new Organization()
            {
                Name = "Org6",
                External_org_unit_id = "6"
            };
            var org7 = new Organization()
            {
                Name = "Org7",
                External_org_unit_id = "7"
            };

            //Act
            _service.AddOrganization(org5);
            _service.AddOrganization(org6);
            _service.AddOrganization(org7);

            //Assert
            // Assert.AreEqual(3, _context..Count);
        }

        [TestMethod]
        public void CreateOrg_Success_MultipleSameName()
        {
            //Arrange
            _service = new ReachMeeService(_reachMeeClient);
            var org1 = new Organization()
            {
                Name = "Org"
            };


            //Act
            _service.AddOrganization(org1);
            _service.AddOrganization(org1);
            _service.AddOrganization(org1);

            //Assert
            // Assert.AreEqual(3, _context..Count);
        }

        [TestMethod]
        public void CreateOrg_Fails_DuplicateExternalId()
        {
            //Arrange
            _service = new ReachMeeService(_reachMeeClient);
            var org1 = new Organization()
            {
                Name = "Org1",
                External_org_unit_id = "1"
            };

            //Act

            //Assert
            Assert.ThrowsException<System.Exception>(() => _service.AddOrganization(org1)); //Due to a known bug in ReachMee API, we get error 500 instead of 409
            //Assert.ThrowsException<OrganizationAlreadyExistsException>(() => _service.AddOrganization(org1));
        }
        #endregion

        #region Get Tests
        [TestMethod]
        public void GetOrganizations_Success()
        {
            //Arrange
            var context = new ReachMeeContextInMemory();
            _service = new ReachMeeService(new ReachMeeClientInMemory(context));
            var org1 = new Organization()
            {
                Name = "Org1"
            };
            context.AddOrganization(org1);
            context.AddOrganization(org1);
            context.AddOrganization(org1);

            //Act
            List<Organization> result = _service.GetOrganizations();

            //Assert
            Assert.AreEqual(3, result.Count);
        }

        [DataTestMethod]
        [DataRow("1")]
        [DataRow("2")]
        [DataRow("3")]
        [DataRow("4")]
        public void GetSpecificOrganization_ByExternalId_Success(string external_id)
        {
            //Arrange
            _service = new ReachMeeService(_reachMeeClient);


            //Act
            var result = _service.GetOrganization(external_id);


            //Assert
            Assert.AreEqual(external_id, result[0].External_org_unit_id);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        public void GetSpecificOrganization_ByInternalId_Success(int internal_id)
        {
            //Arrange
            var context = new ReachMeeContextInMemory();
            _service = new ReachMeeService(new ReachMeeClientInMemory(context));

            context.AddOrganization(new Organization()
            {
                Name = "Org1",
                External_org_unit_id = "1"
            });
            context.AddOrganization(new Organization()
            {
                Name = "Org2",
                External_org_unit_id = "2"
            });
            context.AddOrganization(new Organization()
            {
                Name = "Org3",
                External_org_unit_id = "3"
            });
            context.AddOrganization(new Organization()
            {
                Name = "Org4",
                External_org_unit_id = "4"
            });


            //Act
            var result = _service.GetOrganization(internal_id);


            //Assert
            Assert.AreEqual(internal_id, result[0].Org_unit_id);
        }
        #endregion
    }
}
