using MGH.EF.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace MGH.EF.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        //simple select
        // var customers = _dbContext
        //     .Customers
        //     .TagWith("customersQuery without cars")
        //     .ToList();

        //select with eager loading 
        // var customers = _dbContext
        //     .Customers
        //     .Include(a => a.Cars)
        //     .TagWith("eager loading with cars")
        //     .ToList();

        //get all cars of a customer explicitly eager loading
        // var customer = _dbContext
        //     .Customers
        //     .FirstOrDefault(a => a.FirstName == "ali");
        // _dbContext
        //     .Entry(customer)
        //     .Collection(a => a.Cars)
        //     .Load();
        
        
        //get all cars of a customer explicitly lazy loading
        var post = _dbContext
            .Posts
            .FirstOrDefault(a => a.Title.Contains("abc"));
       
        var cars = post?.Comments.ToList();  
        //unit12-------------------------------------------------------------------
        //behineh 
        var records = _dbContext
            .Posts
            .Where(a => a.Title.Contains("ali"))
            .Select(a => new
            {
                Fullname = $"{a.Title} {a.Text}"
            })
            .ToList();


        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}