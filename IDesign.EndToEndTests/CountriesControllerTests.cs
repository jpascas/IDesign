using IDesign.Manager.Models;
using NFluent;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IDesign.EndToEndTests;

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

    [Test]
    public async Task Post_AddsNewCountry()
    {
        var token = await UsersControllerTests.AuthenticateAsAdmin(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var newCountry = new { Name = "TestCountry" };
        var response = await _client.PostAsJsonAsync("/api/countries", newCountry);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Check.That(content).Contains("TestCountry");
    }

    [Test]
    public async Task Post_ReturnsBadRequest_WhenModelIsInvalid()
    {
        var token = await UsersControllerTests.AuthenticateAsAdmin(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var invalidCountry = new { Name = "" }; // Name required
        var response = await _client.PostAsJsonAsync("/api/countries", invalidCountry);
        Check.That(response.StatusCode).Is(System.Net.HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Put_UpdatesCountry()
    {
        var token = await UsersControllerTests.AuthenticateAsAdmin(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var updatedCountry = new { Id = 1, Name = "USA Updated" };
        var response = await _client.PutAsJsonAsync("/api/countries/1", updatedCountry);
        Check.That(response.StatusCode).Is(System.Net.HttpStatusCode.NoContent);

        // Verify update
        var getResponse = await _client.GetAsync("/api/countries/1");
        var content = await getResponse.Content.ReadAsStringAsync();
        Check.That(content).Contains("USA Updated");
    }

    [Test]
    public async Task Put_ReturnsBadRequest_WhenIdMismatch()
    {
        var token = await UsersControllerTests.AuthenticateAsAdmin(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var updatedCountry = new { Id = 999, Name = "Mismatch" };
        var response = await _client.PutAsJsonAsync("/api/countries/1", updatedCountry);
        Check.That(response.StatusCode).Is(System.Net.HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Delete_RemovesCountry()
    {
        var token = await UsersControllerTests.AuthenticateAsAdmin(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Add a country to delete
        var newCountry = new { Name = "ToDelete" };
        var postResponse = await _client.PostAsJsonAsync("/api/countries", newCountry);
        postResponse.EnsureSuccessStatusCode();
        var created = await postResponse.Content.ReadFromJsonAsync<CountryDto>();
        Check.That(created).IsNotNull();

        // Delete
        var deleteResponse = await _client.DeleteAsync($"/api/countries/{created.Id}");
        Check.That(deleteResponse.StatusCode).Is(System.Net.HttpStatusCode.NoContent);

        // Verify deletion
        var getResponse = await _client.GetAsync($"/api/countries/{created.Id}");
        Check.That(getResponse.StatusCode).Is(System.Net.HttpStatusCode.NotFound);
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }
}