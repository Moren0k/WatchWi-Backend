namespace Watchwi.Domain.Entities.Users;

public enum UserRole
{
    None = 0,
    User = 10,
    Moderator = 50,
    Admin = 100
}

public enum UserPlan
{
    None = 0,
    Basic = 10,
    Standard = 50,
    Premium = 100
}