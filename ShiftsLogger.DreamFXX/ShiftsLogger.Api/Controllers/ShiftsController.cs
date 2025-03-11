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
    public async Task<IActionResult> PutShift()
    {
        var shift = await _context.Shifts.FirstOrDefaultAsync(s => s.EndTime == null);
        if (shift == null)
        {
            return NotFound("No open shifts. Start a new one before ending.");
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
            throw new Exception("An error occured while updating the shift.");
        }

        return NoContent();
    }

    // POST: api/Shifts
    [HttpPost]
    public async Task<ActionResult<Shift>> PostShift(WorkerNameDto workerName)
    {
        var openShift = await _context.Shifts.FirstOrDefaultAsync(s => s.EndTime == null);
        if (openShift != null)
        {
            return BadRequest("Another shift is open right now! Close it first and then start a new one.");

        }

        var shift = new Shift
        {
            WorkerName = workerName.workerName ?? "Unknown",
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
