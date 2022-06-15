using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ProxyApi.OuterApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiProxyController : ControllerBase
    {
        private readonly IInnerApiClient _innerApiClient;

        public ApiProxyController(IInnerApiClient innerApiClient)
        {
            _innerApiClient = innerApiClient;
        }

        [HttpGet]
        [Route("api/get/{**rest}")]
        public async Task<Stream> Get(string rest)
        {
            var url = WebUtility.UrlDecode($"/{rest}{Request.QueryString.Value}");

            var response = await _innerApiClient.GetAsync(url);

            return await response.Content.ReadAsStreamAsync();
        }
    }
}