using GroceryStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Repositories
{
    public interface ICustomerRepository : IDisposable
    {
        
        Task AddCustomerAsync(Customer customer);
        Task<Customer> GetACustomerByIdAsync(int customerId);
        Task<IList<Customer>> GetAllCustomersAsync();
        Task UpdateCustomerAsync(Customer modifiedCustomer);
        Task DeleteCustomerById(int customerId);

    }
}
