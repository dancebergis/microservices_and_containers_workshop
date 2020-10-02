using Kainos.Contexts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Diagnostics;
using System.Linq;

namespace Sandelys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductsController : ControllerBase
    {

        [HttpGet()]
        public IActionResult GetProductsWithPrices()
        {
            var multiplier = Environment.GetEnvironmentVariable("KainuPolitika", EnvironmentVariableTarget.Process);
            try
            {
                IDatabase cache = RedisContext.Connection.GetDatabase();
                var result = cache.SetMembers("items");
                var prices = result.ToList().Select(m =>
                {
                    var product = JsonConvert.DeserializeObject<Product>(m);
                    return new Product()
                    {
                        Name = product.Name,
                        Price = product.Price * decimal.Parse(multiplier)
                    };
                });
                return Ok(prices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
