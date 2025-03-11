using Microsoft.EntityFrameworkCore;
using ShiftsLogger.Api.Data;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()
    .SetBasePath("C:/Users/marti/source/repos/CodeReviews.Console.ShiftsLogger/ShiftsLogger.DreamFXX/ShiftsLogger.Api")
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

builder.Services.AddControllers();
builder.Services.AddDbContext<ShiftsDbContext>(opt =>
    opt.UseSqlServer(config.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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