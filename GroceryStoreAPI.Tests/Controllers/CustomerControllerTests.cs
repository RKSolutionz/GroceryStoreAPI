using Microsoft.VisualStudio.TestTools.UnitTesting;
using GroceryStoreAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using GroceryStoreAPI.Interfaces.Services;
using Moq;
using Microsoft.Extensions.Logging;
using GroceryStoreAPI.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GroceryStoreAPI.Controllers.Tests
{
    [TestClass]
    public class CustomerControllerTests
    {
        Mock<ICustomerService> customerServiceMock;
        Mock<ILogger<CustomerController>> loggerMock;
        CustomerController controller;

        [TestInitialize]
        public void Setup()
        {
            customerServiceMock = new Mock<ICustomerService>();
            loggerMock = new Mock<ILogger<CustomerController>>();
            controller = new CustomerController(customerServiceMock.Object, loggerMock.Object);
        }

        [TestMethod()]
        public async Task AddCustomer_EmptyDatabaseTest()
        {
            customerServiceMock.Setup(m => m.GetAllCustomersAsync())
                .ReturnsAsync(new List<Customer>());

            customerServiceMock.Setup(m => m.AddCustomerAsync("test"))
                .ReturnsAsync(new Customer() { Id = 1, Name = "test" })
                .Verifiable();

            var actionResult = (await controller.AddCustomerAsync("test")).Result as OkObjectResult;

            Customer actualCustomer = (Customer)actionResult.Value;

            Assert.AreEqual(1, actualCustomer.Id);
            Assert.AreEqual("test", actualCustomer.Name);

        }

        [TestMethod]
        public async Task AddCustomer_NonEmptyDatabaseTest()
        {
            customerServiceMock.Setup(m => m.GetAllCustomersAsync())
                .ReturnsAsync(new List<Customer>() { new Customer() { Id = 1, Name = "Test1" } });

            customerServiceMock.Setup(m => m.AddCustomerAsync("Test2"))
                .ReturnsAsync(new Customer() { Id = 2, Name = "Test2" })
                .Verifiable();

            var actionResult = (await controller.AddCustomerAsync("Test2")).Result as OkObjectResult;

            Customer actualCustomer = (Customer)actionResult.Value;

            Assert.AreEqual(2, actualCustomer.Id);
            Assert.AreEqual("Test2", actualCustomer.Name);

        }

        [TestMethod]
        public async Task AddCustomer_EmptyCustomerName()
        {
            var actionResult = (await controller.AddCustomerAsync(string.Empty)).Result as ObjectResult;

            Assert.AreEqual(400, actionResult.StatusCode);
            Assert.AreEqual("Customer Name is empty", actionResult.Value);
        }

        [TestMethod]
        public async Task GetCustomerById_NotExistingID_Test()
        {
            Customer customer = null;
            customerServiceMock.Setup(m => m.GetACustomerByIdAsync(1))
                .ReturnsAsync(customer);

            var actionResult = (await controller.GetCustomerByIdAsync(1)).Result as ObjectResult;

            Assert.AreEqual(404, actionResult.StatusCode);
            Assert.AreEqual($"Customer with id : 1 does not exist", actionResult.Value);
        }

        [TestMethod]
        public async Task GetCustomerById_ExistingID_Test()
        {
            customerServiceMock.Setup(m => m.GetACustomerByIdAsync(1))
                .ReturnsAsync(new Customer() { Id = 1, Name = "Test" });

            var actionResult = (await controller.GetCustomerByIdAsync(1)).Result as OkObjectResult;

            Customer actualCustomer = (Customer)actionResult.Value;

            Assert.AreEqual(200, actionResult.StatusCode);
            Assert.AreEqual(1, actualCustomer.Id);
            Assert.AreEqual("Test", actualCustomer.Name);
        }

        [TestMethod()]
        public async Task GetCustomers_ListOfTwo_Test()
        {
            customerServiceMock.Setup(m => m.GetAllCustomersAsync())
              .ReturnsAsync(new List<Customer>()
              {
                  new Customer() { Id = 1, Name = "Test1" },
                  new Customer() {Id =2, Name ="Test2" }
              });

            var actionResult = (await controller.GetCustomersAsync()).Result as OkObjectResult;

            IList<Customer> actualCustomerList = (IList<Customer>)actionResult.Value;

            Assert.AreEqual(200, actionResult.StatusCode);

            Assert.AreEqual(2, actualCustomerList.Count);

            Assert.AreEqual(1, actualCustomerList[0].Id);
            Assert.AreEqual("Test1", actualCustomerList[0].Name);
            Assert.AreEqual(2, actualCustomerList[1].Id);
            Assert.AreEqual("Test2", actualCustomerList[1].Name);
        }

        [TestMethod]
        public async Task GetCustomers_EmptyList_Test()
        {
            customerServiceMock.Setup(m => m.GetAllCustomersAsync())
              .ReturnsAsync(new List<Customer>());

            var actionResult = (await controller.GetCustomersAsync()).Result as ObjectResult;

            IList<Customer> actualCustomerList = (IList<Customer>)actionResult.Value;

            Assert.AreEqual(200, actionResult.StatusCode);
            Assert.AreEqual(0, actualCustomerList.Count);

        }

        [TestMethod]
        public async Task UpdateCustomer_ExistingCustomer_Test()
        {
            Customer customer = new Customer()
            {
                Id = 1,
                Name = "Test3"
            };

            customerServiceMock.Setup(m => m.GetACustomerByIdAsync(1))
                .ReturnsAsync(customer)
                .Verifiable();

            customerServiceMock.Setup(m => m.UpdateCustomerAsync(customer))
               .ReturnsAsync(customer)
               .Verifiable();


            var actionResult = (await controller.UpdateCustomerAsync(customer)).Result as OkObjectResult;

            Customer modifiedCustomer = (Customer)actionResult.Value;

            Assert.AreEqual(1, modifiedCustomer.Id);
            Assert.AreEqual("Test3", modifiedCustomer.Name);
        }

        [TestMethod]
        public async Task UpdateCustomer_Not_ExistingCustomer_Test()
        {
            Customer customer = new Customer()
            {
                Id = 1,
                Name = "Test3"
            };

            customerServiceMock.Setup(m => m.UpdateCustomerAsync(customer))
                 .ThrowsAsync(new KeyNotFoundException($"Customer with {1} Does not exist"))
                 .Verifiable();

            var actionResult = (await controller.UpdateCustomerAsync(customer)).Result as ObjectResult;

            Assert.AreEqual(404, actionResult.StatusCode);
            Assert.AreEqual($"Customer with id : 1 does not exist", actionResult.Value);
        }

        [TestMethod]
        public async Task DeleteCustomer_ExistingCustomer_Test()
        {
            Customer customer = new Customer()
            {
                Id = 1,
                Name = "Test3"
            };

            customerServiceMock.Setup(m => m.DeleteCustomerByIdAsync(1))
                .ReturnsAsync(customer)
                .Verifiable();

            var actionResult = (await controller.DeleteCustomerByIdAsync(1)).Result as OkObjectResult;

            Assert.AreEqual(200, actionResult.StatusCode);

            Customer deletedCustomer = (Customer)actionResult.Value;

            Assert.AreEqual(1, deletedCustomer.Id);
            Assert.AreEqual("Test3", deletedCustomer.Name);
        }

        [TestMethod]
        public async Task DeleteCustomer_Not_ExistingCustomer_Test()
        {
            Customer customer = new Customer()
            {
                Id = 1,
                Name = "Test3"
            };

            customerServiceMock.Setup(m => m.DeleteCustomerByIdAsync(1))
                .ThrowsAsync(new KeyNotFoundException($"Customer with {1} Does not exist"))
                .Verifiable();

            var actionResult = (await controller.DeleteCustomerByIdAsync(1)).Result as ObjectResult;

            Assert.AreEqual(404, actionResult.StatusCode);
            Assert.AreEqual($"Customer with id : 1 does not exist", actionResult.Value);
        }
    }
}