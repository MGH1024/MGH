using MGH.Core.Domain.Concretes;

namespace MGH.EF.Persistence.Entities;

public class Comment : AuditableEntity<int>
{
    public string Name { get; set; }
    public string Text { get; set; }
    
    
    //navigations
    public int PostId { get; set; }
    public virtual Post Post { get; set; }
}