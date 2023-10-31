using MGH.Core.Domain.Concretes;

namespace MGH.Core.Security.Entities;

public class PolicyOperationClaim :AuditableEntity<int>
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