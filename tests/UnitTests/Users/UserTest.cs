using AuthApi.Application.Features.Users;

namespace Tests.UnitTests.Users;

[TestClass]
public class UserTest
{
    [TestMethod]
    public void Create_ValidConstructorInputs_ResultSuccess()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Alberto Junior";
        var email = "alberto@gmail.com";
        var password = "123";
        var role = "manager";

        // Act
        var user = User.Create(id, name, email, password, role);

        // Assert
        Assert.AreEqual(id, user.Value.Id);
        Assert.AreEqual(name, user.Value.Name);
        Assert.AreEqual(email, user.Value.Email);
        Assert.AreEqual(password, user.Value.Password);
        Assert.AreEqual(role, user.Value.Role);
    }

    [TestMethod]
    [DataRow(null, "email", "password", "manager")]
    [DataRow("name", null, "password", "manager")]
    [DataRow("name", "email", null, "manager")]
    [DataRow("name", "email", "password", null)]
    public void Create_InvalidConstructorInputs_ResultError(string name, string email, string password, string role)
    {
        // Arrange

        // Act
        var user = User.Create(Guid.NewGuid(), name, email, password, role);

        // Assert
        Assert.AreEqual(true, user.IsFailure);
    }
}
