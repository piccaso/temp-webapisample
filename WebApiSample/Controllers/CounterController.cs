using System;
using System.Linq;
using System.Threading.Tasks;
using Library;
using Microsoft.AspNetCore.Mvc;

namespace WebApiSample.Controllers
{
    [Route("api/[controller]")]
    public class CounterController : Controller
    {
        private readonly ICounterFactory _counterFactory;

        public CounterController(ICounterFactory counterFactory)
        {
            _counterFactory = counterFactory;
        }

        /// <summary>
        /// get counter value
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>a value</returns>
        [HttpGet("{guid}/[action]")]
        public long Get(Guid guid) => _counterFactory.Create(guid).Get();

        /// <summary>
        /// increment a counter
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        [HttpPatch("{guid}/[action]/{by}")]
        public long Increment(Guid guid, long by = 1) => _counterFactory.Create(guid).Increment(by);

        // also support post (but hide it)
        [HttpPost("{guid}/Increment/{by}"), ApiExplorerSettings(IgnoreApi = true)]
        public long Inc(Guid guid, long by = 1) => _counterFactory.Create(guid).Increment(by);

        /// <summary>
        /// set a counter value
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("{guid}/[action]/{value}")]
        public long Set(Guid guid, long value)
        {
            _counterFactory.Create(guid).Set(value);
            return value;
        }
    }
}
