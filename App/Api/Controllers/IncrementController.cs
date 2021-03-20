using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib;
using MassTransit;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IncrementController : ControllerBase
    {
        readonly IBus _bus;

        public IncrementController(IBus bus)
        {
            _bus = bus;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Lib.KeyValuePair kvp)
        {
            var endPoint = await _bus.GetSendEndpoint(new Uri("queue:input-queue"));
            await endPoint.Send(kvp); 
            return Ok();

        }
    }
}

