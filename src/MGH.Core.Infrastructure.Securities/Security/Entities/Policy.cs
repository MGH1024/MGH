using MGH.Core.Domain.Entities;

namespace MGH.Core.Infrastructure.Securities.Security.Entities;

public class Policy : FullAuditedEntity<int>
{
    public string Title { get; set; }
    
    public virtual ICollection<PolicyOperationClaim> PolicyOperationClaims { get; set; } = null!;

    public Policy(string title)
    {
        Title = title;
    }

    public Policy(int id,string title)
    {
        Id = id;
        Title = title;
    }
}