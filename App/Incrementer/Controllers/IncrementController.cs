using Incrementer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Incrementer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IncrementController : ControllerBase
    {
        private readonly IRepo _repo;
    
        public IncrementController(IRepo repo)
        {
            _repo = repo;

        }

        /// <summary>
        /// Get the current value from the db.
        /// If being called from another controller protect against null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(string key)
        {
            if (key==null)
            {
                return BadRequest("you must pass a value");
            }
            else
            {
                try
                {
                    var record = new Lib.KeyValuePair(key, 0);

                    var data = await _repo.Get(record);


                    return Ok(data);

                }
                catch (System.Exception ex)
                {

                    return StatusCode(StatusCodes.Status500InternalServerError, ex);
                }

            }

        }


    }
}
