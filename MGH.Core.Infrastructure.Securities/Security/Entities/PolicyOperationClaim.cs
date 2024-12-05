using MGH.Core.Domain.BaseEntity;

namespace MGH.Core.Infrastructure.Securities.Security.Entities;

public class PolicyOperationClaim :AuditAbleEntity<int>
{
    public int PolicyId { get; set; }
    public int OperationClaimId { get; set; }

    public virtual Policy Policy { get; set; } = null!;
    public virtual OperationClaim OperationClaim { get; set; } = null!;

    public PolicyOperationClaim(int policyId, int operationClaimId)
    {
        PolicyId = policyId;
        OperationClaimId = operationClaimId;
    }
    
    public PolicyOperationClaim(int id, int policyId, int operationClaimId) : base(id)
    {
        PolicyId = policyId;
        OperationClaimId = operationClaimId;
    }
}