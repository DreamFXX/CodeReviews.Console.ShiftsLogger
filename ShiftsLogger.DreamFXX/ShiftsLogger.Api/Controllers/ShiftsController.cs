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
            return BadRequest("Tato směna již byla ukončena.");
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
            throw new Exception("Při aktualizaci směny došlo k chybě.");
        }

        return NoContent();
    }

    // POST: api/Shifts
    [HttpPost]
    public async Task<ActionResult<Shift>> PostShift(EmployeeDto employeeName)
    {
        var openShift = await _context.Shifts.FirstOrDefaultAsync(s => s.EndTime == null);
        if (openShift != null)
        {
            return BadRequest("Jiná směna je právě otevřená! Nejprve ji ukončete a poté začněte novou.");
        }

        var shift = new Shift
        {
            EmployeeName = employeeName.workerName ?? "Neznámý",
            StartTime = DateTime.Now
        };
        _context.Shifts.Add(shift);

        await _context.SaveChangesAsync();
        return CreatedAtAction("GetShiftById", new { id = shift.Id }, shift);
    }

    // DELETE: api/Shifts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShift(int id)
    {
        var shift = await _context.Shifts.FindAsync(id);

        if (shift == null)
        {
            return NotFound();
        }

        _context.Shifts.Remove(shift);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool ShiftExists(int id)
    {
        return _context.Shifts.Any(e => e.Id == id);
    }
}
