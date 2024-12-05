namespace MGH.Core.Domain.Entity.Audits;

public class AuditLog
{
    public Guid Id { get; set; }
    public string TableName { get; set; }
    public string BeforeData { get; set; }
    public string AfterData { get; set; }
    public string Action { get; set; } 
    public string Username { get; set; }
    public DateTime Timestamp { get; set; }
}