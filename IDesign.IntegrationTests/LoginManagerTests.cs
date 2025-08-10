using IDesign.Manager;
using IDesign.Manager.Models;
using Microsoft.Extensions.DependencyInjection;
using NFluent;

namespace IDesign.IntegrationTests;

[TestFixture]
public class LoginManagerTests
{
    private CustomWebApplicationFactory<Program> _factory = null!;
    private IServiceScope _scope = null!;
    private LoginManager _manager = null!;

    [SetUp]
    public void SetUp()
    {
        _factory = new CustomWebApplicationFactory<Program>();
        _scope = _factory.Services.CreateScope();
        _manager = (LoginManager)_scope.ServiceProvider.GetRequiredService<ILoginManager>();
    }

    [Test]
    public async Task HandleLoginAsync_ReturnsSuccess_WhenCredentialsAreValid()
    {
        var dto = new LoginUserDto { Email = "admin@idesign.test", Password = "admin123" };
        var result = await _manager.HandleLoginAsync(dto);

        Check.That(result.Success).IsTrue();
        Check.That(result.Result).IsNotNull(); // Should be a JWT
    }

    [Test]
    public async Task HandleLoginAsync_ReturnsFailure_WhenUserDoesNotExist()
    {
        var dto = new LoginUserDto { Email = "notfound@idesign.test", Password = "admin123" };
        var result = await _manager.HandleLoginAsync(dto);

        Check.That(result.Success).IsFalse();
        Check.That(result.FailureCode).Is(401);
        Check.That(result.FailureMessages[0]).Is("User doesnt exists");
    }

    [Test]
    public async Task HandleLoginAsync_ReturnsFailure_WhenPasswordIsIncorrect()
    {
        var dto = new LoginUserDto { Email = "admin@idesign.test", Password = "wrongpassword" };
        var result = await _manager.HandleLoginAsync(dto);

        Check.That(result.Success).IsFalse();
        Check.That(result.FailureCode).Is(401);
        Check.That(result.FailureMessages[0]).Is("Password is incorrect");
    }

    [TearDown]
    public void TearDown()
    {
        _scope?.Dispose();
        _factory?.Dispose();
    }
}