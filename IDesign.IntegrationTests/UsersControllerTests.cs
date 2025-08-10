using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using NFluent;
using System.Net.Http.Json;

namespace IDesign.IntegrationTests;

[TestFixture]
public class UsersControllerTests
{
    private CustomWebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {        
        _factory = new CustomWebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [SetUp]
    public void SetUp()
    {
        _client.DefaultRequestHeaders.Authorization = null; // Clear any previous authorization headers
    }

    [Test]
    public async Task Login_ReturnsToken_WhenCredentialsAreValid()
    {
        await AuthenticateAsAdmin();
    }

    public async Task<string> Authenticate(string email, string password)
    {
        var loginRequest = new
        {
            Email = email,
            Password = password
        };
        var response = await _client.PostAsJsonAsync("/api/users/login", loginRequest);
        response.EnsureSuccessStatusCode();
        var token = await response.Content.ReadAsStringAsync();
        Check.That(token).IsNotNull();
        Check.That(token).Contains("eyJ"); // crude JWT check
        return token;
    }

    public async Task<string> AuthenticateAsAdmin()
    {
        return await Authenticate("admin@idesign.test", "admin123");
    }

    public async Task<string> AuthenticateAsNormalUser()
    {
        return await Authenticate("user1@idesign.test", "user123");
    }



    [Test]
    public async Task Login_ReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        var loginRequest = new
        {
            Email = "admin@idesign.test",
            Password = "123"
        };
        var response = await _client.PostAsJsonAsync("/api/users/login", loginRequest);
        Check.That(response.StatusCode).IsEqualTo(System.Net.HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task UsersOnly_ReturnsUnauthorized_WhenCredentialsAreInvalid()
    {
        var response = await _client.GetAsync("/api/users/usersonly");
        Check.That(response.StatusCode).IsEqualTo(System.Net.HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task UsersOnly_Returns200_WhenCredentialsAreValid()
    {
        var token = await AuthenticateAsNormalUser();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync("/api/users/usersonly");
        Check.That(response.StatusCode).IsEqualTo(System.Net.HttpStatusCode.OK);
    }

    [Test]
    public async Task AdminOnly_Returns200_WhenRoleIsAdmin()
    {
        var token = await AuthenticateAsAdmin();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync("/api/users/adminonly");
        Check.That(response.StatusCode).IsEqualTo(System.Net.HttpStatusCode.OK);
    }

    [Test]
    public async Task AdminOnly_Returns403_WhenRoleIsNotAdmin()
    {
        var token = await AuthenticateAsNormalUser();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync("/api/users/adminonly");
        Check.That(response.StatusCode).IsEqualTo(System.Net.HttpStatusCode.Forbidden);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }
    // Add more tests for authenticated scenarios as needed
}
