using MGH.Core.Domain.Abstracts;
using MGH.Core.Domain.Concretes;

namespace MGH.EF.Persistence.Entities;

public class Post : AuditableEntity<int>, IPageable, IDropdownAble
{
    public string Title { get; set; }
    public string Text { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Tag> Tags { get; set; }

    public int Row { get; }
    public int TotalCount { get; }
    public int CurrentPage { get; }
    public int PageSize { get; }
    public string ListItemText { get; }
    public string ListItemTextForAdmins { get; }
}