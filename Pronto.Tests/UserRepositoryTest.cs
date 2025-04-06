using Xunit;
using Dapper;
using Pronto.Models;
using Pronto.Repositories;
using System.Threading.Tasks;
using System.Net.Sockets;

public class UserRepositoryTest : RepositoryTestBase
{
    private readonly UserRepository _userRepository;

    public UserRepositoryTest() : base()
    {
        _userRepository = new UserRepository(new DatabaseHelper(Configuration));
    }

    [Fact]
    public void CreateUser_SavesToDatabase()
    {
        var business = new Business
        {
            Name = "Business Name",
            Address = "123 Business Address",
            Email = "testbusiness@example.com",
            PhoneNumber = "555-1234"
        };

        var businessId = _connection.ExecuteScalar<int>(
        "INSERT INTO business (Name, Address, Email, PhoneNumber) VALUES (@Name, @Address, @Email, @PhoneNumber); SELECT LAST_INSERT_ID();", business);


        var user = new User 
        { 
            BusinessId = businessId, 
            Email = "test@example.com", 
            PasswordHash = "hashedPW"
        };

        var userId = _connection.ExecuteScalar<int>(
               "INSERT INTO user (BusinessId, Email, PasswordHash) VALUES (@BusinessId, @Email, @PasswordHash); SELECT LAST_INSERT_ID();", user);

        var savedUser = _connection.QuerySingleOrDefault<User>(
            "SELECT * FROM user WHERE UserId = @UserId", new { UserId = userId });

        Assert.NotNull(savedUser);
        Assert.Equal(user.Email, savedUser.Email);
        Assert.Equal(user.BusinessId, savedUser.BusinessId);
        //Assert.AreEqual(user.CreatedAt.Date, savedUser.CreatedAt.Date);
    }
}
