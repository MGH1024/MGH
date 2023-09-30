namespace MGH.Domain.Entities;

public class Person: Entity<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
}