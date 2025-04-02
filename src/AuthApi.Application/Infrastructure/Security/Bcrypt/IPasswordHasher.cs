namespace AuthApi.Application.Infrastructure.Security.Bcrypt;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}
