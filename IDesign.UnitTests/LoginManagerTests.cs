using IDesign.Access.Entities;
using IDesign.Access.Identity;
using IDesign.Access.Identity.JWT;
using IDesign.Access.Repositories;
using IDesign.Manager;
using IDesign.Manager.Models;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using NFluent;

namespace IDesign.UnitTests;

[TestFixture]
public class LoginManagerTests
{
    private Mock<IUserRepository> _userRepo = null!;
    private Mock<IJwtProvider> _jwtProvider = null!;
    private Mock<IPasswordHasher> _passwordHasher = null!;
    private LoginManager _manager = null!;

    [SetUp]
    public void SetUp()
    {
        _userRepo = new Mock<IUserRepository>();
        _jwtProvider = new Mock<IJwtProvider>();
        _passwordHasher = new Mock<IPasswordHasher>();
        _manager = new LoginManager(_userRepo.Object, _jwtProvider.Object, _passwordHasher.Object);
    }

    [Test]
    public async Task HandleLoginAsync_ReturnsSuccess_WhenCredentialsAreValid()
    {
        var user = new User { Id = 1, Email = "user@domain.com", PasswordHash = "hashed" };
        var dto = new LoginUserDto { Email = "user@domain.com", Password = "password" };

        _userRepo.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync(user);
        _passwordHasher.Setup(h => h.VerifyHashedPassword(user.PasswordHash, dto.Password)).Returns(true);
        _jwtProvider.Setup(j => j.Generate(user)).Returns("jwt-token");

        var result = await _manager.HandleLoginAsync(dto);

        Check.That(result.Success).IsTrue();
        Check.That(result.Result).Is("jwt-token");
    }

    [Test]
    public async Task HandleLoginAsync_ReturnsFailure_WhenUserDoesNotExist()
    {
        var dto = new LoginUserDto { Email = "notfound@domain.com", Password = "password" };
        _userRepo.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync((User?)null);

        var result = await _manager.HandleLoginAsync(dto);

        Check.That(result.Success).IsFalse();
        Check.That(result.FailureCode).Is(401);
        Check.That(result.FailureMessages[0]).Is("User doesnt exists");
    }

    [Test]
    public async Task HandleLoginAsync_ReturnsFailure_WhenPasswordIsIncorrect()
    {
        var user = new User { Id = 1, Email = "user@domain.com", PasswordHash = "hashed" };
        var dto = new LoginUserDto { Email = "user@domain.com", Password = "wrongpassword" };

        _userRepo.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync(user);
        _passwordHasher.Setup(h => h.VerifyHashedPassword(user.PasswordHash, dto.Password)).Returns(false);

        var result = await _manager.HandleLoginAsync(dto);

        Check.That(result.Success).IsFalse();
        Check.That(result.FailureCode).Is(401);
        Check.That(result.FailureMessages[0]).Is("Password is incorrect");
    }
}