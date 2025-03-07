using Microsoft.EntityFrameworkCore;
using ShiftsLoggerAPI.Models;

namespace ShiftsLoggerAPI.Data;
public class ShiftsDbContext : DbContext
{
    public ShiftsDbContext(DbContextOptions<ShiftsDbContext> options) : base(options) { }
    public DbSet<Shift> Shifts { get; set; }
}
