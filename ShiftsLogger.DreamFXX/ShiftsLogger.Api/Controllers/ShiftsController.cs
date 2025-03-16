using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShiftsLogger.Api.Data;
using ShiftsLogger.Api.Models;

namespace ShiftsLogger.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShiftsController : ControllerBase
{
    private readonly ShiftsDbContext _context;
    public ShiftsController(ShiftsDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Shift>>> GetAllShifts()
    {
        return await _context.Shifts.ToListAsync();
    }

    [HttpGet("active")]
    public async Task<ActionResult<Shift>> GetActiveShift()
    {
        var activeShift = await _context.Shifts.FirstOrDefaultAsync(s => s.EndTime == null);
        if (activeShift == null)
        {
            return NotFound("Žádná aktivní směna nebyla nalezena.");
        }
        return activeShift;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<object>> GetStatistics()
    {
        var shifts = await _context.Shifts.ToListAsync();
        
        var totalShifts = shifts.Count;
        var completedShifts = shifts.Count(s => s.EndTime != null);
        var activeShifts = shifts.Count(s => s.EndTime == null);
        
        var totalHours = shifts
            .Where(s => s.Duration.HasValue)
            .Sum(s => s.Duration.Value.TotalHours);
        
        var averageShiftDuration = completedShifts > 0 
            ? shifts
                .Where(s => s.Duration.HasValue)
                .Average(s => s.Duration.Value.TotalHours) 
            : 0;

        return new
        {
            TotalShifts = totalShifts,
            CompletedShifts = completedShifts,
            ActiveShifts = activeShifts,
            TotalHours = Math.Round(totalHours, 2),
            AverageShiftDuration = Math.Round(averageShiftDuration, 2)
        };
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Shift>> GetShiftById(int id)
    {
        var shift = await _context.Shifts.FindAsync(id);
        if (shift == null)
        {
            return NotFound();
        }
        return shift;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutShift(int id)
    {
        var shift = await _context.Shifts.FindAsync(id);
        if (shift == null)
        {
            return NotFound("Směna nebyla nalezena.");
        }

        if (shift.EndTime != null)
        {
            return BadRequest("This shift has already been ended.");
        }

        shift.EndTime = DateTime.Now;
        shift.Duration = shift.EndTime - shift.StartTime;

        _context.Entry(shift).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new Exception("An error occurred while updating the shift.");
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Shift>> PostShift(EmployeeDto employeeDto)
    {
        var openShift = await _context.Shifts.FirstOrDefaultAsync(s => s.EndTime == null);
        if (openShift != null)
        {
            return BadRequest("Another shift is open right now! End it first, before starting a new one.");
        }

        var shift = new Shift
        {
            EmployeeName = employeeDto.EmployeeName,
            StartTime = DateTime.Now
        };
        _context.Shifts.Add(shift);

        await _context.SaveChangesAsync();
        return CreatedAtAction("GetShiftById", new { id = shift.Id }, shift);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShift(int id)
    {
        var shift = await _context.Shifts.FindAsync(id);

        if (shift == null)
        {
            return NotFound("Shift not found.");
        }

        _context.Shifts.Remove(shift);

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
