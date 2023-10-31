using MGH.Core.Domain.Concretes;

namespace MGH.Core.Security.Entities;

public class Policy :AuditableEntity<int>
{
    public string Title { get; set; }
}