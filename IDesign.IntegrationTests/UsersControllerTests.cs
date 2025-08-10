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

    [SetUp]
    public void SetUp()
    {
        _factory = new CustomWebApplicationFactory<Program>();        
        _client = _factory.CreateClient();
    }

    [Test]
    public async Task Login_ReturnsToken_WhenCredentialsAreValid()
    {
        var loginRequest = new {
            Email = "admin@idesign.test",
            Password = "admin123"
        };
        var response = await _client.PostAsJsonAsync("/api/users/login", loginRequest);
        response.EnsureSuccessStatusCode();
        var token = await response.Content.ReadAsStringAsync();
        Check.That(token).IsNotNull();
        Check.That(token).Contains("eyJ"); // crude JWT check
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
    public async Task ProtectedEndpoint_RequiresAuthentication()
    {
        var response = await _client.GetAsync("/api/users/usersonly");
        Check.That(response.StatusCode).IsEqualTo(System.Net.HttpStatusCode.Unauthorized);
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }
    // Add more tests for authenticated scenarios as needed
}
