using MGH.Core.Domain.Concretes;

namespace MGH.EF.Persistence.Entities;

public class Tag :AuditableEntity<int>
{
    public string Title { get; set; }
    
    //navigations
    public virtual ICollection<Post> Posts { get; set; }
}