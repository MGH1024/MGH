using MGH.EF.API.Config;
using MGH.EF.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sqlConnection = builder
    .Configuration
    .GetSection("DatabaseConnection")
    .Get<DatabaseConnection>()
    .SqlConnection;

builder.Services
    .AddDbContext<AppDbContext>(options => options
        .UseSqlServer(sqlConnection,
            a => a.MigrationsAssembly("MGH.EF.API")));

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