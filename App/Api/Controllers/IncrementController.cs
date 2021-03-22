using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
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

        /// <summary>
        /// Accepts body of {Key:string, Value:decimal} and sends the 
        /// body into the rabbitMq queue
        /// </summary>
        /// <param name="kvp"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Lib.KeyValuePair kvp)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Please check you request body, something is not correct");
            }
            else if (kvp.Value < 0)
            {
                return BadRequest("Did you mean /Decrement?");
            }
            else if (kvp.Value == 0)
            {
                return Ok("This endpoint does not increment by 0");
            }

            var endPoint = await _bus.GetSendEndpoint(new Uri("queue:kvpQueue"));

            try
            {
                await endPoint.Send(kvp);

                return Ok($"Value: {kvp.Value} has been queued for key: {kvp.Key}");
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

    }
}

