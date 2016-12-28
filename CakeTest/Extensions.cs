using System.Diagnostics;
using System.Web.Http;
using System.Web.Http.Results;

namespace CakeTest
{
    public static class Extensions
    {
        public static OkNegotiatedContentResult<T> ToOk<T>(this T t, ApiController controller)
        {
            var result = new OkNegotiatedContentResult<T>(t, controller);
            return result;
        }
    }
}