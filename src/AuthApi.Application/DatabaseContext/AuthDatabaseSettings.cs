namespace AuthApi.Application.DatabaseContext;

public class AuthDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public DatabaseCollections DatabaseCollections { get; set; } = null!;
};

public class DatabaseCollections
{
    public string UsersCollection { get; set; } = null!;
};
