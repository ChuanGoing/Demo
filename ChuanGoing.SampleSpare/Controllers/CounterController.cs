using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChuanGoing.Samples.Controllers
{
    [Route("api/[controller]")]
    public class CounterController : ControllerBase
    {
        private static int _count = 0;

        [HttpGet]
        [AllowAnonymous]
        public string Count()
        {
            return $"Count {(_count < int.MaxValue ? ++_count : int.MaxValue)} from SampleSpareApi";
        }
    }
}
