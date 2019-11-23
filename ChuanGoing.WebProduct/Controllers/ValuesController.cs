using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ChuanGoing.WebProduct.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static int _count = 0;

        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "This is ProductApi Authorize action result!";
        }

        [HttpGet("Count")]
        [AllowAnonymous]
        public string Count()
        {
            return $"Count {++_count} from ProductApi";
        }
    }
}
