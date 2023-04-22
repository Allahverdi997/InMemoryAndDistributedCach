using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemoryAndDistributedCaching
{
    public class RedisService
    {
        public ConnectionMultiplexer connectionMultiplexer { get; set; }
        public void Connect() 
        {
            connectionMultiplexer = ConnectionMultiplexer.Connect("localhost:6379"); 
        }
        public IDatabase GetDatabase(int db) 
        { 
            return connectionMultiplexer.GetDatabase(db); 
        }
    }
}
