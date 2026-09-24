using server.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        List<Product> products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Wi-Fi",
                Price = 5550
            },
            new Product
            {
                Id = 2,
                Name = "Keyboard",
                Price = 300
            }
        };

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            Product? product = products.Find(p => p.Id == id);

            if(product == null)
                return NotFound();

            return Ok(product);
        }
    }
}
