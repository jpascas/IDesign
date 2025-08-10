using NFluent;
using System.Net.Http.Headers;

namespace IDesign.IntegrationTests;

[TestFixture]
public class CitiesControllerTests
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
        var response = await _client.GetAsync("/api/cities");        
        Check.That(response.StatusCode).Is(System.Net.HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Get_ReturnsCitiesList()
    {
        var token = await UsersControllerTests.AuthenticateAsNormalUser(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync("/api/cities");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Check.That(content).IsNotNull();
        Check.That(content).Contains("City"); // crude check for seeded data
    }

    [Test]
    public async Task GetById_ReturnsCity()
    {
        var token = await UsersControllerTests.AuthenticateAsNormalUser(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync("/api/cities/1");
        Check.That(response.IsSuccessStatusCode).IsTrue();
    }

    [Test]
    public async Task GetById_ReturnsNotFound()
    {
        var token = await UsersControllerTests.AuthenticateAsNormalUser(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync($"/api/cities/{int.MaxValue}");
        Check.That(response.StatusCode).Is(System.Net.HttpStatusCode.NotFound);
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }
}