using ShiftsLoggerAPI.Data;
namespace ShiftsLoggerAPI.Services;


public class ShiftService : IShiftService
{
  private readonly ShiftsDbContext _dbContext;

  public ShiftService(ShiftsDbContext dbContext)
  {
    _dbContext = dbContext;
  }


}
