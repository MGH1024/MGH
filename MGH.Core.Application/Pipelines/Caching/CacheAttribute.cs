namespace MGH.Core.Application.Pipelines.Caching;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class CacheAttribute : Attribute
{
    public int CacheDuration { get; set; }
    public string EntityName { get; set; }
}