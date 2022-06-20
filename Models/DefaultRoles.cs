namespace CP.Api.Models;

public static class DefaultRoles
{
    public static readonly Role Administrator = new Role { Id = 1, Name = "Administrator" };
    public static readonly Role User = new Role { Id = 2, Name = "User" };
    public static readonly Role Manager = new Role { Id = 3, Name = "Manager" };
}