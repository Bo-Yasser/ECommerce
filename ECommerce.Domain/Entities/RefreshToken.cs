namespace ECommerce.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; private set; }
    public DateTimeOffset ExpiresOn { get; private set; }
    public DateTimeOffset? RevokedOn { get; private set; }

    public Guid UserId { get; private set; }

    public bool IsExpired => DateTimeOffset.UtcNow >= ExpiresOn;

    public bool IsActive => RevokedOn == null && !IsExpired && !IsDeleted;

    private RefreshToken() { }

    private RefreshToken(string token, DateTimeOffset expiresOn, Guid userId)
    {
        Id = Guid.NewGuid();
        Token = token;
        ExpiresOn = expiresOn;
        UserId = userId;
    }

    public static RefreshToken Create(string token, DateTimeOffset expiresOn, Guid userId)
    {
        return new RefreshToken(token, expiresOn, userId);
    }

    public void Revoke()
    {
        RevokedOn = DateTimeOffset.UtcNow;
    }
}