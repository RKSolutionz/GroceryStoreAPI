using Microsoft.VisualStudio.TestTools.UnitTesting;
using GroceryStoreAPI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using GroceryStoreAPI.Repositories;
using Moq;
using Microsoft.Extensions.Logging;
using GroceryStoreAPI.Interfaces.Services;
using GroceryStoreAPI.Models;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Services.Tests
{
    [TestClass]
    public class CustomerServiceTests
    {

        Mock<ICustomerRepository> customerRepositoryMock;
        Mock<ILogger<CustomerService>> loggerMock;
        ICustomerService customerService;

        [TestInitialize]
        public void Setup()
        {
            customerRepositoryMock = new Mock<ICustomerRepository>();
            loggerMock = new Mock<ILogger<CustomerService>>();
            customerService = new CustomerService(customerRepositoryMock.Object, loggerMock.Object);
        }


        [TestMethod]
        public async Task GetACustomersByIdAsync_NonEmptyList_Test()
        {
            Customer customer = new Customer()
            {
                Id = 1,
                Name = "Test"
            };

            customerRepositoryMock.Setup(m => m.GetACustomerByIdAsync(1))
             .ReturnsAsync(customer)
             .Verifiable();

            Customer customerResult = await customerService.GetACustomerByIdAsync(1);

            Assert.AreEqual(1, customerResult.Id);
            Assert.AreEqual("Test", customerResult.Name);
        }

        [TestMethod]
        public async Task GetACustomersByIdAsync_EmptyList_Test()
        {
            Customer customer = null;

            customerRepositoryMock.Setup(m => m.GetACustomerByIdAsync(1))
             .ReturnsAsync(customer)
             .Verifiable();

            Customer customerResult = await customerService.GetACustomerByIdAsync(1);

            Assert.IsNull(customerResult);
        }


        [TestMethod]
        public async Task GetAllCustomersAsync_NonEmptyList_Test()
        {
            Customer customer = new Customer()
            {
                Id = 1,
                Name = "Test"
            };

            customerRepositoryMock.Setup(m => m.GetAllCustomersAsync())
             .ReturnsAsync(new List<Customer>() { customer })
             .Verifiable();

            IList<Customer> customerList = await customerService.GetAllCustomersAsync();

            Assert.AreEqual(1, customerList.Count);
            Assert.AreEqual(1, customerList[0].Id);
            Assert.AreEqual("Test", customerList[0].Name);
        }

        [TestMethod]
        public async Task GetAllCustomersAsync_EmptyList_Test()
        {
            customerRepositoryMock.Setup(m => m.GetAllCustomersAsync())
             .ReturnsAsync(new List<Customer>())
             .Verifiable();

            IList<Customer> customerList = await customerService.GetAllCustomersAsync();

            Assert.AreEqual(0, customerList.Count);

        }

        [TestMethod]
        public async Task UpdateCustomerAsync_Existing_Test()
        {
            Customer customer = new Customer()
            {
                Id = 1,
                Name = "Test1"
            };

            Customer modifiedCustomer = new Customer()
            {
                Id = 1,
                Name = "Test2"
            };

            customerRepositoryMock.Setup(m => m.GetACustomerByIdAsync(1))
                .ReturnsAsync(customer)
                .Verifiable();

            customerRepositoryMock.Setup(m => m.UpdateCustomerAsync(modifiedCustomer))
                .Verifiable();

            var result = await customerService.UpdateCustomerAsync(modifiedCustomer);

            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Test2", result.Name);

        }

        [TestMethod]
        public async Task UpdateCustomerAsync_NotExisting_Test()
        {
            Customer customer = new Customer()
            {
                Id = 10,
                Name = "Test"
            };

            customerRepositoryMock.Setup(m => m.GetACustomerByIdAsync(1))
                .ReturnsAsync(customer)
                .Verifiable();

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => { await customerService.UpdateCustomerAsync(customer); },$"Customer with {customer.Id} Does not exist");

        }

        [TestMethod]
        public async Task DeleteCustomerByIdAsync_Existing_Test()
        {
            Customer customer = new Customer()
            {
                Id = 1,
                Name = "Test1"
            };

            customerRepositoryMock.Setup(m => m.GetACustomerByIdAsync(1))
                .ReturnsAsync(customer)
                .Verifiable();

            customerRepositoryMock.Setup(m => m.DeleteCustomerById(1))
               .Verifiable();

            var result = await customerService.DeleteCustomerByIdAsync(1);

            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Test1", result.Name);


        }

        [TestMethod]
        public async Task DeleteCustomerByIdAsync_NotExisting_Test()
        {
            Customer customer = null;

            customerRepositoryMock.Setup(m => m.GetACustomerByIdAsync(1))
                .ReturnsAsync(customer)
                .Verifiable();

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => { await customerService.DeleteCustomerByIdAsync(1); }, $"Customer with id: 1 not exist");
        }

        [TestMethod]
        public async Task AddCustomerAsync()
        {
            Customer customer = new Customer()
            {
                Name = "Test"
            };

            customerRepositoryMock.Setup(m => m.AddCustomerAsync(customer))
                .Verifiable();

            Customer newCustomer = await customerService.AddCustomerAsync("Test");

            Assert.AreEqual("Test", newCustomer.Name);

        }
    }
}