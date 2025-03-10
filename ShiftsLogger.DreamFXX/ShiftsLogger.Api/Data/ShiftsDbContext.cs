using Microsoft.EntityFrameworkCore;
using ShiftsLogger.Api.Models;

namespace ShiftsLogger.Api.Data;

public class ShiftsDbContext : DbContext
{
    public ShiftsDbContext(DbContextOptions<ShiftsDbContext> options) : base(options)
    {
    }

    public DbSet<Shift> Shifts { get; set; }
}
