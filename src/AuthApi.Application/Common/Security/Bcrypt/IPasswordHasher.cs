namespace AuthApi.Application.Common.Security.Bcrypt;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}
