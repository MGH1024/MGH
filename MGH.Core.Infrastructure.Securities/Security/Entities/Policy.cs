using MGH.Core.Domain.BaseEntity;

namespace MGH.Core.Infrastructure.Securities.Security.Entities;

public class Policy :AuditAbleEntity<int>
{
    public string Title { get; set; }
    
    public virtual ICollection<PolicyOperationClaim> PolicyOperationClaims { get; set; } = null!;

    public Policy(string title)
    {
        Title = title;
    }

    public Policy(int id,string title) :base(id)
    {
        Title = title;
    }
}