using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InMemoryAndDistributedCaching.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public readonly IMemoryCache Memory;
        public IDistributedCache RedisCache { get; set; }
        public RedisService RedisService { get; set; }
        public WeatherForecastController(IMemoryCache memory,IDistributedCache redisCache,RedisService redisService)
        {
            Memory = memory;
            RedisCache = redisCache;
            RedisService = redisService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            
            var model = new List<Person>();
            var output = new List<Person>();
            
            var response = Memory.Get("persons");
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(5);
            options.SlidingExpiration = TimeSpan.FromSeconds(60);

            if (response == null)
            {
                Thread.Sleep(60000);
                Memory.Set("persons", 1,options);
            }

            return Ok(Memory.Get("persons"));
        }
        [HttpGet("redis")]
        public IActionResult GetRedis()
        {

            var model = new List<Person>();
            var output = new List<Person>();
            IDatabase database = RedisService.GetDatabase(1);
            var response = database.StringGet("alma");
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(5);
            options.SlidingExpiration = TimeSpan.FromSeconds(60);

            if (response.IsNull)
            {
                Thread.Sleep(6000);
                database.StringSet("alma", "1");
            }
            var a = database.StringGet("alma");
            return Ok(a.ToString());
        }
        [HttpGet("salam")]
        public IActionResult GetSalam()
        {
            return Ok();
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
