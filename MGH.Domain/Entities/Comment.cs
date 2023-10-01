namespace MGH.Domain.Entities;

public class Comment : Entity<int>
{
    public string Name { get; set; }
    public string Text { get; set; }
    
    
    //navigations
    public int PostId { get; set; }
    public virtual Post Post { get; set; }
}