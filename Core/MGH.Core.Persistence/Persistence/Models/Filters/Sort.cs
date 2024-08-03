namespace MGH.Core.Infrastructure.Persistence.Persistence.Models.Filters;

public class Sort(string field, string dir)
{
    public string Field { get; set; } = field;
    public string Dir { get; set; } = dir;
}
