namespace FSI.CloudShopping.Domain.Entities;

using FSI.CloudShopping.Domain.Core;

/// <summary>
/// Entity representing authentication credentials for backoffice/admin access control.
/// </summary>
public class Authentication : Entity<int>
{
    public string Email { get; private set; } = string.Empty;
    public bool AuthorizedAccess { get; private set; }

    protected Authentication() { }

    public Authentication(int id, string email, bool authorizedAccess, DateTime createdAt) : base(id)
    {
        Email = email;
        AuthorizedAccess = authorizedAccess;
    }

    public static Authentication Create(string email, bool authorizedAccess = false)
        => new Authentication(0, email, authorizedAccess, DateTime.UtcNow);

    public void Authorize()
    {
        AuthorizedAccess = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Revoke()
    {
        AuthorizedAccess = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
