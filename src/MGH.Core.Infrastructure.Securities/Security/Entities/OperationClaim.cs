using MGH.Core.Domain.Entities;

namespace MGH.Core.Infrastructure.Securities.Security.Entities;

public class OperationClaim : AuditedEntity<int>
{
    public string Name { get; set; }

    public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; } = null!;
    public virtual ICollection<PolicyOperationClaim> PolicyOperationClaims { get; set; } = null!;

    public OperationClaim()
    {
        Name = string.Empty;
    }

    public OperationClaim(string name)
    {
        Name = name;
    }

    public OperationClaim(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
