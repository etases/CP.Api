namespace CP.Api.Models;

public static class DefaultRoles
{
    public const string AdministratorString = "Administrator";
    public const string UserString = "User";
    public const string ManagerString = "Manager";
    public static readonly Role Administrator = new() {Id = 1, Name = AdministratorString};
    public static readonly Role User = new() {Id = 2, Name = UserString};
    public static readonly Role Manager = new() {Id = 3, Name = ManagerString};
}