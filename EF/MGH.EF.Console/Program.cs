using MGH.EF.Console.Extensions;
using MGH.EF.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, EF!");

ServiceRegistration.RegisterServices();
var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(ServiceRegistration.GetConnectionString(),
        a => a.MigrationsAssembly("MGH.EF.Console"))
    .Options;

var appDbContext = new AppDbContext(options);