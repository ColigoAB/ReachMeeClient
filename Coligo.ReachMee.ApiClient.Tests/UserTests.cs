using Coligo.ReachMee.Data.ApiClients;
using Coligo.ReachMee.Data.Exceptions;
using Coligo.ReachMee.Data.Interfaces;
using Coligo.ReachMee.Data.Models;
using Coligo.ReachMee.Data.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Coligo.ReachMee.Tests
{
    [TestClass]
    public class UserTests
    {
        ReachMeeService _service;
        readonly IApiClient _reachMeeClient = new ReachMeeClientInMemory();

        [TestMethod]
        public void CreateUser_Success()
        {
            //Arrange

            _service = new ReachMeeService(_reachMeeClient);
            var user1 = new User()
            {
                First_name = "TestUser",
                Surname = "Surname",
                Employee_number = "1",
                User_name = "1"
            };
            var user2 = new User()
            {
                First_name = "TestUser",
                Surname = "Surname",
                Employee_number = "2",
                User_name = "2"
            };
            var user3 = new User()
            {
                First_name = "TestUser",
                Surname = "Surname",
                Employee_number = "3",
                User_name = "3"
            };

            //Act
            _service.AddUser(user1);
            _service.AddUser(user2);
            _service.AddUser(user3);

            //Assert
            // As no exception is thrown, this is OK.
            // We do NOT want to depend on Get queries as well
        }

        [TestMethod]
        public void CreateUser_Fails_Duplicate_Username()
        {
            //Arrange
            _service = new ReachMeeService(_reachMeeClient);
            var user = new User()
            {
                First_name = "TestUser",
                Surname = "Surname",
                Employee_number = "1",
                User_name = "1"
            };
            _service.AddUser(user);
            user.Employee_number = "2";

            //Act
            //Assert
            Assert.ThrowsException<UserAlreadyExistsException>(() => _service.AddUser(user));
        }

        [TestMethod]
        public void CreateUser_Fails_Duplicate_EmployeeNumber()
        {
            //Arrange
            _service = new ReachMeeService(_reachMeeClient);
            var user = new User()
            {
                First_name = "TestUser",
                Surname = "Surname",
                Employee_number = "1",
                User_name = "1"
            };
            _service.AddUser(user);
            user.User_name = "2";

            //Act
            //Assert
            Assert.ThrowsException<UserAlreadyExistsException>(() => _service.AddUser(user));
        }

        [TestMethod]
        public void CreateUser_Fails_Duplicate_Username_And_EmployeeNumber()
        {
            //Arrange
            _service = new ReachMeeService(_reachMeeClient);
            var user = new User()
            {
                First_name = "TestUser",
                Surname = "Surname",
                Employee_number = "1",
                User_name = "1"
            };
            _service.AddUser(user);

            //Act
            //Assert
            Assert.ThrowsException<UserAlreadyExistsException>(() => _service.AddUser(user));
        }

        [TestMethod]
        public void GetUser_Success()
        {
            //Arrange
            _service = new ReachMeeService(_reachMeeClient);

            int i = 0;
            while (i < 100)
            {
                var user = new User()
                {
                    First_name = $"TestUser_{i}",
                    Surname = $"Surname_{i}",
                    Employee_number = $"{i}",
                    User_name = $"User_{i}"
                };
                i++;
                _service.AddUser(user);
            }



            //Act
            var result = _service.GetUsers(page_size: 50);

            //Assert
            Assert.AreEqual(100, result.Count);
        }

        [TestMethod]
        public void GetUser_SpecificEmployeeNumber_Success()
        {
            //Arrange
            _service = new ReachMeeService(_reachMeeClient);

            int i = 0;
            while (i < 100)
            {
                var user = new User()
                {
                    First_name = $"TestUser_{i}",
                    Surname = $"Surname_{i}",
                    Employee_number = $"{i}",
                    User_name = $"User_{i}"
                };
                i++;
                _service.AddUser(user);
            }



            //Act
            var result = _service.GetUsers(employee_number: "17");

            //Assert
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetUser_SpecificUserName_Success()
        {
            //Arrange
            _service = new ReachMeeService(_reachMeeClient);

            int i = 0;
            while (i < 100)
            {
                var user = new User()
                {
                    First_name = $"TestUser_{i}",
                    Surname = $"Surname_{i}",
                    Employee_number = $"{i}",
                    User_name = $"User_{i}"
                };
                i++;
                _service.AddUser(user);
            }



            //Act
            var result = _service.GetUsers(user_name: "User_17");

            //Assert
            Assert.AreEqual(1, result.Count);
        }
    }
}
