using GroceryStoreAPI.DBContext;
using GroceryStoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {

        #region Private Fields

        private readonly GroceryContext _dbContext;
        private readonly ILogger _logger;
        bool disposed = false;

        #endregion

        #region Ctor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext">DBContext</param>
        /// <param name="logger">ILogger</param>
        public CustomerRepository(GroceryContext dbContext, ILogger<CustomerRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a Customer
        /// </summary>
        /// <param name="customer">Customer Record</param>
        /// <returns></returns>
        public async Task AddCustomerAsync(Customer customer)
        {
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Get a Customer By Id
        /// </summary>
        /// <param name="customerId">Customer Id</param>
        /// <returns></returns>
        public async Task<Customer> GetACustomerByIdAsync(int customerId)
        {
            return await _dbContext.Customers.FindAsync(customerId);
        }

        /// <summary>
        /// Get List of Customers
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Customer>> GetAllCustomersAsync()
        {
            return await _dbContext.Customers.ToListAsync();
        }

        /// <summary>
        /// Update Customer
        /// </summary>
        /// <param name="modifiedCustomer"></param>
        /// <returns></returns>
        public async Task UpdateCustomerAsync(Customer modifiedCustomer)
        {
            _dbContext.Customers.Update(modifiedCustomer);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Delete a Customer by Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task DeleteCustomerById(int customerId)
        {
            Customer customer = await _dbContext.Customers.FindAsync(customerId);

            _dbContext.Customers.Remove(customer);
            await _dbContext.SaveChangesAsync();

        }

        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        #endregion
    }
}
