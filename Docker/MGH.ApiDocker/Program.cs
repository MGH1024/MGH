using MGH.ApiDocker.Extensions;
using MGH.Core.Application.HttpClient.Configurations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var httpClientFakeService =
    builder.Configuration.GetSection("ExternalServices:HttpClientFakeService").Get<ExternalServiceInfo>();
builder.Services.RegisterHttpClientService(httpClientFakeService);

builder.Services.Configure<ExternalServiceInfo>(
    builder.Configuration.GetSection("ExternalServices:NamedHttpClientFakeService"));
var namedHttpClientFakeService =
    builder.Configuration.GetSection("ExternalServices:NamedHttpClientFakeService").Get<ExternalServiceInfo>();
builder.Services.RegisterNamedHttpClientService(namedHttpClientFakeService);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();