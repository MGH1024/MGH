namespace MGH.Core.Infrastructure.Persistence.EF;

public record AuditInterceptorDto(string Username,string IpAddress,DateTime Now);