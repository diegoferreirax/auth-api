namespace AuthApi.Domain.Enums;

public static class RoleCodes
{
    public const string UserManager = "UM";

    private static readonly HashSet<string> ValidCodes = [UserManager];

    public static bool IsValid(string code) => ValidCodes.Contains(code);
}
