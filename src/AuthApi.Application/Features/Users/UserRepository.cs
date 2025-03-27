using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users;

public sealed class UserRepository
{
    public Maybe<User> Get(string username, string password)
    {
        var users = new List<User>()
        {
            User.Create(Guid.NewGuid(), "batman", "batman@email.com", "batman", "manager").Value,
            User.Create(Guid.NewGuid(), "robin", "robin@email.com", "robin", "employee").Value
        };

        return users.Where(x => x.Name.ToLower() == username.ToLower() && x.Password == password).FirstOrDefault();
    }
}
