using System;
using Gamykla.Contexts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Gamykla.Controllers
{
    [Route("api/")]
    [ApiController]
    public class GamyklaController : ControllerBase
    {
        

        [Route("Gamykla/Gamink/{name}/{price}")]
        public IActionResult Gamink(string name, int price)
        {
            var value = "";
            try
            {
                IDatabase cache = RedisContext.Connection.GetDatabase();
                value = JsonConvert.SerializeObject(new { Name = name, Price = price });
                cache.SetAdd("items", value);
                cache.KeyExpire($"items", new TimeSpan(23, 59, 59));  // https://redis.io/commands/expire Set all keys to not live for more than 24 hours
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("{Ka tik pagaminta: " + value + "}");
        }
    }
}