using DotNetCardsServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace DotNetCardsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        static private List<Customer> myCustomers = new List<Customer>() { new Customer(1, "Tzach",15), new Customer(2, "Avi",40), new Customer(3, "Mor",30) };

        // GET: api/customers
        [HttpGet]
        public IActionResult Get(int? maxAge=120, int? minAge = 0)
        {
          
                var filteredCustomers = myCustomers.Where((c) => c.Age > minAge&&c.Age<maxAge);
                return Ok(filteredCustomers);
            
        }
       


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Customer? c = myCustomers.Find(customer => customer.Id == id);
            if(c == null)
            {
                return NotFound();
            }

            return Ok(c);
        }


        [HttpPost]
        public IActionResult Post([FromBody]  Customer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }
            myCustomers.Add(customer);

            return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
        }







    }
}
