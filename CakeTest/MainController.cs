using System.Web.Http;

namespace CakeTest
{
    [Route("api")]
    public class MainController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            return "Hello, world".ToOk(this);
        }
    }
}