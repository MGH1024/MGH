namespace MGH.Domain.Entities;

public class Tag :Entity<int>
{
    public string Title { get; set; }
    
    //navigations
    public virtual ICollection<Post> Posts { get; set; }
}