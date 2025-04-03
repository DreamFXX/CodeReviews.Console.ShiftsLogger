using ShiftsLoggerAPI.Models;

namespace ShiftsLoggerAPI.Services;

public interface IShiftService
{
    Task<IEnumerable<Shift>> GetAllShiftsAsync();
    Task<Shift?> GetShiftByIdAsync(int id);
    Task<Shift> CreateShiftAsync(Shift shift);
    Task<Shift?> UpdateShiftAsync(Shift shift);
    Task<bool> DeleteShiftAsync(int id);
}