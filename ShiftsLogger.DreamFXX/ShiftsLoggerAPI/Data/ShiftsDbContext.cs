using Microsoft.EntityFrameworkCore;
using ShiftsLoggerAPI.Models;


public class ShiftsDbContext : DbContext

{
    public ShiftsDbContext(DbContextOptions<ShiftsDbContext> options) : base(options) { }


    public DbSet<Shift> Shifts { get; set; }
}