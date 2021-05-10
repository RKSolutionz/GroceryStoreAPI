using GroceryStoreAPI.DBContext;
using GroceryStoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Helpers
{
    public class SampleData
    {
        /// <summary>
        /// Adding Sample Data for Customers
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void Init(IServiceProvider serviceProvider)
        {
            using (var context = new GroceryContext(serviceProvider.GetRequiredService<DbContextOptions<GroceryContext>>()))
            {
                if (context.Customers.Any())
                {
                    return;
                }

                context.Customers.AddRange(

                new Customer()
                {
                    Id = 1,
                    Name = "Bob"
                },

                new Customer()
                {
                    Id = 2,
                    Name = "Mary"
                },

                new Customer()
                {
                    Id = 3,
                    Name = "Joe"
                });

                context.SaveChanges();
            }
        }
    }
}
