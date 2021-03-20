using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IncrementController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Lib.KeyValuePair kvp)
        {
            return Ok();
        }


    }
}

