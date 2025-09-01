using MGH.Core.Domain.BaseModels;

namespace MGH.Core.Infrastructure.Securities.Security.Entities;

public class Policy :Entity<int>
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