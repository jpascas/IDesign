using IDesign.Manager.Models;
using NFluent;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using IDesign.IntegrationTests;

namespace IDesign.EndToEndTests;

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

    [Test]
    public async Task Post_AddsNewCity()
    {
        var token = await UsersControllerTests.AuthenticateAsAdmin(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var newCity = new { Name = "TestCity", CountryId = 1 };
        var response = await _client.PostAsJsonAsync("/api/cities", newCity);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Check.That(content).Contains("TestCity");
    }

    [Test]
    public async Task Post_ReturnsBadRequest_WhenModelIsInvalid()
    {
        var token = await UsersControllerTests.AuthenticateAsAdmin(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var invalidCity = new { Name = "", CountryId = 1 }; // Name required
        var response = await _client.PostAsJsonAsync("/api/cities", invalidCity);
        Check.That(response.StatusCode).Is(System.Net.HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Put_UpdatesCity()
    {
        var token = await UsersControllerTests.AuthenticateAsAdmin(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var updatedCity = new { Id = 1, Name = "City Updated", CountryId = 1 };
        var response = await _client.PutAsJsonAsync("/api/cities/1", updatedCity);
        Check.That(response.StatusCode).Is(System.Net.HttpStatusCode.NoContent);

        // Verify update
        var getResponse = await _client.GetAsync("/api/cities/1");
        var content = await getResponse.Content.ReadAsStringAsync();
        Check.That(content).Contains("City Updated");
    }

    [Test]
    public async Task Put_ReturnsBadRequest_WhenIdMismatch()
    {
        var token = await UsersControllerTests.AuthenticateAsAdmin(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var updatedCity = new { Id = 999, Name = "Mismatch", CountryId = 1 };
        var response = await _client.PutAsJsonAsync("/api/cities/1", updatedCity);
        Check.That(response.StatusCode).Is(System.Net.HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Delete_RemovesCity()
    {
        var token = await UsersControllerTests.AuthenticateAsAdmin(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Add a city to delete
        var newCity = new { Name = "ToDelete", CountryId = 1 };
        var postResponse = await _client.PostAsJsonAsync("/api/cities", newCity);
        postResponse.EnsureSuccessStatusCode();
        var created = await postResponse.Content.ReadFromJsonAsync<CityDto>();
        Check.That(created).IsNotNull();

        // Delete
        var deleteResponse = await _client.DeleteAsync($"/api/cities/{created.Id}");
        Check.That(deleteResponse.StatusCode).Is(System.Net.HttpStatusCode.NoContent);

        // Verify deletion
        var getResponse = await _client.GetAsync($"/api/cities/{created.Id}");
        Check.That(getResponse.StatusCode).Is(System.Net.HttpStatusCode.NotFound);
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }
}