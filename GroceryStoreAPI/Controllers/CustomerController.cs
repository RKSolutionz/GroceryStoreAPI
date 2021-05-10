using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroceryStoreAPI.Interfaces.Services;
using GroceryStoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GroceryStoreAPI.Controllers
{
    /// <summary>
    /// Customer Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger _logger;

        /// <summary>
        /// Customer Controller Constructor
        /// </summary>
        /// <param name="customerService"> Service Object</param>
        /// <param name="logger">ILogger object</param>
        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        /// <summary>
        /// Heart Beat
        /// </summary>
        /// <returns>Running : If the API is up and running</returns>
        [Route("hc")]
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult HeartBeat()
        {
            _logger.LogInformation("Running");
            return Ok("Running");
        }

        /// <summary>
        /// Add a Customer record
        /// </summary>
        /// <param name="customerName">Name of the Customer</param>
        /// <returns>New Customer Record</returns>
        [HttpPost("add")]
        [Produces(typeof(Customer))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Customer>> AddCustomerAsync([FromBody] string customerName)
        {
            Customer customer;
            try
            {
                if (string.IsNullOrEmpty(customerName))
                {
                    _logger.LogError($"Customer Name is empty");
                    return BadRequest($"Customer Name is empty");
                }

                customer = await _customerService.AddCustomerAsync(customerName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }

            return Ok(customer);
        }

        /// <summary>
        /// Get a Customer By Idendtifier
        /// </summary>
        /// <param name="id">Customer Identifier</param>
        /// <returns>Customer Record</returns>
        [HttpGet("{id:int}")]
        [Produces(typeof(Customer))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomerByIdAsync(int id)
        {
            Customer customer;
            try
            {
                customer = await _customerService.GetACustomerByIdAsync(id);

                if (customer == null)
                {
                    _logger.LogInformation($"Customer with id : {id} does not exist");
                    return NotFound($"Customer with id : {id} does not exist");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching Customer", ex.Message);
                return BadRequest(ex);
            }

            return Ok(customer);
        }

        /// <summary>
        /// Get a list of all Customers
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        [Produces(typeof(IEnumerable<Customer>))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomersAsync()
        {
            IEnumerable<Customer> customerList;
            try
            {
                customerList = await _customerService.GetAllCustomersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }

            return Ok(customerList);
        }

        /// <summary>
        /// Update Customer record
        /// </summary>
        /// <param name="modifiedCustomer">Modified Customer Object</param>
        /// <returns>Updated Customer Record</returns>
        [HttpPut("update")]
        [Produces(typeof(Customer))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Customer>> UpdateCustomerAsync(Customer modifiedCustomer)
        {
            Customer customer;
            try
            {
                customer = await _customerService.UpdateCustomerAsync(modifiedCustomer);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"Customer with id : {modifiedCustomer.Id} does not exist",ex?.Message);
                return NotFound($"Customer with id : {modifiedCustomer.Id} does not exist");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating customer : {modifiedCustomer.Id}", ex.Message);
                return Problem($"Error updating customer : {modifiedCustomer.Id}", ex.Message, statusCode: 400);
            }

            return Ok(customer);
        }

        /// <summary>
        /// Delete Customer By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Customer>> DeleteCustomerByIdAsync(int id)
        {
            Customer customer;
            try
            {
               customer = await _customerService.DeleteCustomerByIdAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"Customer with id : {id} does not exist", ex?.Message);
                return NotFound($"Customer with id : {id} does not exist");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Deleting customer: {id}", ex.Message);
                return Problem($"Error Deleting customer : {id}", ex.Message, statusCode: 400);
            }

            return Ok(customer);
        }
    }
}
