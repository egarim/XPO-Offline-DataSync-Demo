using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BIT.Xpo.Sync.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        // GET api/values

        string osPlatform;
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            string framework = "";
            try
            {

                framework = Assembly
                    .GetEntryAssembly()?
                    .GetCustomAttribute<TargetFrameworkAttribute>()?
                    .FrameworkName;
                osPlatform = System.Runtime.InteropServices.RuntimeInformation.OSDescription;

            }
            catch (Exception)
            {

                throw;
            }
            return new string[] { "Hello", "XPO", "Rest", "BIT.Xpo.OfflineDataSync", $"Framework:{framework} platform:{osPlatform}" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}