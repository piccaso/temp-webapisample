using System;
using System.Linq;
using System.Threading.Tasks;
using Library;
using Microsoft.AspNetCore.Mvc;

namespace WebApiSample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CounterController : Controller
    {
        private readonly ICounter _counter;

        public CounterController(ICounter counter)
        {
            _counter = counter;
        }
        /// <summary>
        /// get counter value
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>a value</returns>
        [HttpGet]
        public long Get(Guid guid) => _counter.Get(guid);

        /// <summary>
        /// increment a counter
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        [HttpPost]
        public long Increment(Guid guid, long by) => _counter.Increment(guid, by);

        /// <summary>
        /// set a counter value
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public long Set(Guid guid, long value) => _counter.Set(guid, value);
    }
}
