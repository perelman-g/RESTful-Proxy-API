using Microsoft.AspNetCore.Mvc;

namespace ProxyApi.InnerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InnerApiController : ControllerBase
    {
        [HttpGet]
        [Route("get")]
        public string GetResponse(string @string, int @int, DateTime date)
        {
            return $"response from inner api: stringParam={@string}, intParam={@int}, dateParam={date.ToString("s")}";
        }
    }
}