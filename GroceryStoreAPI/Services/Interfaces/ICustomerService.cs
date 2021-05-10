using GroceryStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<Customer> AddCustomerAsync(string customerName);
        Task<Customer> GetACustomerByIdAsync(int customerId);
        Task<IList<Customer>> GetAllCustomersAsync();
        Task<Customer> UpdateCustomerAsync(Customer modifiedCustomer);
        Task<Customer> DeleteCustomerByIdAsync(int customerId);
    }
}
