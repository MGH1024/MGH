namespace MGH.Domain.Entities;

public class Car : Entity<int>
{
    public string Name { get; set; }
    public string ModelYear { get; set; }
}