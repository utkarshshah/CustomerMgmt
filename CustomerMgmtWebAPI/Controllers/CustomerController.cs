using CustomerMgmtWebAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CustomerMgmtWebAPI.Controllers
{
    [RoutePrefix("Customer")]
    public class CustomerController : ApiController
    {
        private static List<Customer> customers = new List<Customer>();

        [HttpPost]
        [Route("Add")]
        public IHttpActionResult PostCustomers(List<Customer> newCustomers)
        {
            if (newCustomers == null || newCustomers.Count == 0)
            {
                return BadRequest("No customers provided.");
            }

            foreach (var customer in newCustomers)
            {
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(customer, null, null);
                bool isValid = Validator.TryValidateObject(customer, context, validationResults, true);

                if (!isValid)
                {
                    return BadRequest(string.Join(", ", validationResults.Select(r => r.ErrorMessage)));
                }

                if (customers.Any(c => c.Id == customer.Id))
                {
                    return BadRequest($"ID {customer.Id} already exists.");
                }

                // Insert customer in a sorted manner (Without using built-in sorting)
                int index = 0;
                while (index < customers.Count &&
                       string.Compare(customers[index].LastName, customer.LastName, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    index++;
                }
                while (index < customers.Count &&
                       string.Compare(customers[index].LastName, customer.LastName, StringComparison.OrdinalIgnoreCase) == 0 &&
                       string.Compare(customers[index].FirstName, customer.FirstName, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    index++;
                }

                customers.Insert(index, customer);
            }
            return Ok("Customers added successfully.");
        }

        [HttpGet]
        [Route("Get")]
        public IHttpActionResult GetCustomers()
        {
            if (customers.Count == 0)
            {
                return NotFound();
            }

            return Ok(customers);
        }
    }
}