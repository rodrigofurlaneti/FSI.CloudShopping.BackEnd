namespace FSI.CloudShopping.Domain.Entities;

using FSI.CloudShopping.Domain.Core;

/// <summary>
/// Entity representing an audit log entry for tracking system changes.
/// </summary>
public class AuditLog : Entity<Guid>
{
    public string EntityName { get; private set; }
    public string EntityId { get; private set; }
    public AuditAction Action { get; private set; }
    public string? OldValues { get; private set; }
    public string? NewValues { get; private set; }
    public Guid? UserId { get; private set; }
    public string? UserEmail { get; private set; }
    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }

    public AuditLog(Guid id, string entityName, string entityId, AuditAction action,
        string? oldValues = null, string? newValues = null, Guid? userId = null,
        string? userEmail = null, string? ipAddress = null, string? userAgent = null)
        : base(id)
    {
        EntityName = entityName;
        EntityId = entityId;
        Action = action;
        OldValues = oldValues;
        NewValues = newValues;
        UserId = userId;
        UserEmail = userEmail;
        IpAddress = ipAddress;
        UserAgent = userAgent;
    }

    protected AuditLog() { }

    public static AuditLog Create(string entityName, string entityId, AuditAction action,
        string? oldValues = null, string? newValues = null, Guid? userId = null,
        string? userEmail = null, string? ipAddress = null, string? userAgent = null)
    {
        return new AuditLog(Guid.NewGuid(), entityName, entityId, action, oldValues, newValues,
            userId, userEmail, ipAddress, userAgent);
    }
}

public enum AuditAction
{
    Create = 0,
    Update = 1,
    Delete = 2,
    Approve = 3,
    Reject = 4,
    Export = 5
}
