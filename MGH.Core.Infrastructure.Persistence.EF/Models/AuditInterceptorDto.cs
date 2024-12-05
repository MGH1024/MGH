namespace MGH.Core.Infrastructure.Persistence.EF.Models;

public record AuditInterceptorDto(string Username,string IpAddress,DateTime Now);