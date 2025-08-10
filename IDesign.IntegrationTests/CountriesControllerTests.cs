using NFluent;
using System.Net.Http.Headers;

namespace IDesign.IntegrationTests;

[TestFixture]
public class CountriesControllerTests
{
    private CustomWebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [SetUp]
    public void SetUp()
    {
        _factory = new CustomWebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [Test]
    public async Task Get_ReturnsUnauthorized_WhenCredentialsAreInvalid()
    {
        var response = await _client.GetAsync("/api/countries");
        Check.That(response.StatusCode).Is(System.Net.HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Get_ReturnsCountriesList()
    {
        var token = await UsersControllerTests.AuthenticateAsNormalUser(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync("/api/countries");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Check.That(content).IsNotNull();
        Check.That(content).Contains("USA"); // crude check for seeded data
    }

    [Test]
    public async Task GetById_ReturnsCountry()
    {
        var token = await UsersControllerTests.AuthenticateAsNormalUser(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync("/api/countries/1");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Check.That(response.IsSuccessStatusCode).IsTrue();
        Check.That(content).IsNotNull();
        Check.That(content).Contains("USA");
    }

    [Test]
    public async Task GetById_ReturnsNotFound()
    {
        var token = await UsersControllerTests.AuthenticateAsNormalUser(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync($"/api/countries/{int.MaxValue}");
        Check.That(response.StatusCode).Is(System.Net.HttpStatusCode.NotFound);
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }
}