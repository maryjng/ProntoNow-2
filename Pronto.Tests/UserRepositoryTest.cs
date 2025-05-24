using Xunit;
using Dapper;
using Moq;
using System.Data;
using Pronto.Repositories;
using Pronto.Data;
using Pronto.Models;
using FluentAssertions;

namespace Pronto.Tests;

public class UserRepositoryTests
{
    private readonly Mock<UserRepository> _sut;

    private readonly Mock<IDbConnection> _dbConnection = new();
    private readonly Mock<IDatabaseHelper> _databaseHelper = new();

    public UserRepositoryTests()
    {
        _sut = new Mock<UserRepository>(() => new UserRepository(_databaseHelper.Object));
        _sut.CallBase = true;

        _databaseHelper
            .Setup(s => s.CreateConnection())
            .Returns(_dbConnection.Object);
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsUser_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        var expectedUser = new User
        {
            UserId = userId,
            Email = "test@example.com",
            BusinessId = 123,
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow
        };

        _sut
            .Setup(repo => repo.QuerySingleOrDefaultAsync<User>(
                It.IsAny<IDbConnection>(),
                It.IsAny<string>(),
                It.IsAny<object>()))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _sut.Object.GetUserByIdAsync(userId);

        // Assert
        result.Success.Should().BeTrue("We expect the API response to have returned true with a valid user");
        expectedUser.Should().BeEquivalentTo(result.Data, "The expected user that was mocked should have been wrapped in the API response");
    }

    [Fact]
    public async Task GetUserByIdAsync_ReturnsError_WhenNoUserExists()
    {
        // Arrange
        _sut
            .Setup(repo => repo.QuerySingleOrDefaultAsync<User>(
                It.IsAny<IDbConnection>(),
                It.IsAny<string>(),
                It.IsAny<object>()))
            .ReturnsAsync(() => null);

        // Act
        var result = await _sut.Object.GetUserByIdAsync(1);

        // Assert
        result.Success.Should().BeFalse("We expect the API response to have returned false with an invalid user");
        result.ErrorMessage.Should().Be("User not found.");
        result.StatusCode.Should().Be(404, "The user wasn't found so a 404 should be returned");
        result.Data.Should().BeNull("We don't expect any user data to be present");
    }
}