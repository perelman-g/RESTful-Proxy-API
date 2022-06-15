using ProxyApi.OuterApi;

namespace ProxyApi.Tests
{
    public class Tests
    {
        private const string InnerApiBaseUrl = "https://localhost:4321";
        private HttpClient _innerApiClient = null!;
        private HttpClient _outerApiClient = null!;
        private Fixture _fixture = null!;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();

            var innerApi = new WebApplicationFactory<InnerApiProgram>();
            _innerApiClient = innerApi.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri(InnerApiBaseUrl)
            });

            var outerApi = new WebApplicationFactory<OuterApiProgram>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddTransient<IInnerApiClient>(_ => new InnerApiClient(_innerApiClient));
                    });
                });

            _outerApiClient = outerApi.CreateClient();
        }

        [Test]
        public async Task CanCallInnerApiDirectly()
        {
            var @string = _fixture.Create<string>();
            var @int = _fixture.Create<int>();
            var date = _fixture.Create<DateTime>().ToString("s");

            var response = await _innerApiClient.GetAsync($"/InnerApi/get?string={@string}&int={@int}&date={date}");

            response.EnsureSuccessStatusCode();

            var actual = await response.Content.ReadAsStringAsync();

            var expected = $"response from inner api: stringParam={@string}, intParam={@int}, dateParam={date}";

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task CanCallInnerApiViaOuterApiProxy()
        {
            var @string = _fixture.Create<string>();
            var @int = _fixture.Create<int>();
            var date = _fixture.Create<DateTime>().ToString("s");

            var response = await _outerApiClient.GetAsync($"/ApiProxy/api/get/InnerApi/get?string={@string}&int={@int}&date={date}");

            response.EnsureSuccessStatusCode();

            var actual = await response.Content.ReadAsStringAsync();

            var expected = $"response from inner api: stringParam={@string}, intParam={@int}, dateParam={date}";

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}