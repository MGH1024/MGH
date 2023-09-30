using MGH.EF.Persistence.Contexts;
using MGH.EF.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

// ServiceRegistration.RegisterServices();
var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(ServiceRegistration.GetConnectionString())
    .Options;

var appDbContext = new AppDbContext(options);