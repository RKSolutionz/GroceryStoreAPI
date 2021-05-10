using GroceryStoreAPI.Interfaces.Services;
using GroceryStoreAPI.Models;
using GroceryStoreAPI.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Services
{
    /// <summary>
    /// Customer Service
    /// </summary>
    public class CustomerService : ICustomerService
    {
        #region Private Fields

        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger _logger;

        #endregion

        #region Ctor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="customerRepository">Repository</param>
        /// <param name="logger">ILogger</param>
        public CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Add Customer to Database
        /// </summary>
        /// <param name="customerName">Customer Name</param>
        /// <returns>New Customer Record</returns>
        public async Task<Customer> AddCustomerAsync(string customerName)
        {
            Customer customer;

            try
            {
                customer = new Customer { Name = customerName };
                await _customerRepository.AddCustomerAsync(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }

            return customer;
        }

        /// <summary>
        /// Get a Customer Record by Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>Customer</returns>
        public async Task<Customer> GetACustomerByIdAsync(int customerId)
        {
            Customer customer;
            try
            {
                _logger.LogInformation($"Get Customer by Id : {customerId}");

                customer = await _customerRepository.GetACustomerByIdAsync(customerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }

            return customer;
        }

        /// <summary>
        /// Get list of customers
        /// </summary>
        /// <returns>Customer List</returns>
        public async Task<IList<Customer>> GetAllCustomersAsync()
        {
            IList<Customer> customers;
            try
            {
                _logger.LogInformation($"Get All Customers");
                customers = await _customerRepository.GetAllCustomersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }


            return customers;
        }

        /// <summary>
        /// Update Customer Record to Database
        /// </summary>
        /// <param name="modifiedCustomer">Modified Customer Record</param>
        /// <returns>Modified Customer</returns>
        public async Task<Customer> UpdateCustomerAsync(Customer modifiedCustomer)
        {
            Customer customer;

            try
            {
                customer = await _customerRepository.GetACustomerByIdAsync(modifiedCustomer.Id);

                if (customer == null)
                {
                    throw new KeyNotFoundException($"Customer with {modifiedCustomer.Id} Does not exist");
                }

                customer.Name = modifiedCustomer.Name;

                await _customerRepository.UpdateCustomerAsync(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }

            return modifiedCustomer;
        }

        /// <summary>
        /// Delete a Customer by Id
        /// </summary>
        /// <param name="id">Customer Identifier</param>
        /// <returns>Deleted Customer</returns>
        public async Task<Customer> DeleteCustomerByIdAsync(int id)
        {
            Customer customer;
            try
            {
                _logger.LogInformation($"Delete Customer by Id : {id}");

                customer = await _customerRepository.GetACustomerByIdAsync(id);

                if (customer == null)
                {
                    throw new KeyNotFoundException($"Customer with id: {id} does not exist");
                }

                await _customerRepository.DeleteCustomerById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }

            return customer;
        }
        #endregion
    }
}
