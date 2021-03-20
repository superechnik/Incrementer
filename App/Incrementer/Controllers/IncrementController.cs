using Incrementer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text.Json;

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

        [HttpGet("{key}")]
        public async Task<Models.KeyValue> Get(string key)
        {
            try
            {

                return await _repo.Get(key);

            }
            catch (System.Exception)
            {

                throw;
            }

        }


    }
}
