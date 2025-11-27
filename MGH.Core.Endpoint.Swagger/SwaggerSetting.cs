namespace MGH.Core.Endpoint.Swagger;

public class SwaggerSettings
{
    public string Title { get; set; } = "API";
    public string Version { get; set; } = "v1";
    public string Description { get; set; } = "";
    public string RoutePrefix { get; set; } = "swagger";
    public string BearerDescription { get; set; } = "";
    public SwaggerContact? Contact { get; set; }
    public SwaggerLicense? License { get; set; }
}

public class SwaggerContact
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Url { get; set; }
}

public class SwaggerLicense
{
    public string? Name { get; set; }
    public string? Url { get; set; }
}
