namespace MGH.Core.Application.Pipelines.Authorization;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class RolesAttribute(params string[] roles) : Attribute
{
    public string[] Roles { get; } = roles;
}