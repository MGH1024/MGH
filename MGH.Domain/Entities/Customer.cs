namespace MGH.Domain.Entities;

public class Customer: Entity<Guid>,IPageable,IDropdownAble
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public int Row { get; }
    public int TotalCount { get; }
    public int CurrentPage { get; }
    public int PageSize { get; }
    public string ListItemText { get; }
    public string ListItemTextForAdmins { get; }
}