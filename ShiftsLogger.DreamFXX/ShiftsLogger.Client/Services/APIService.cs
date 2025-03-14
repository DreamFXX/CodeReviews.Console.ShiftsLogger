using System.Text.Json;
using ShiftsLogger.Client.Models;

namespace ShiftsLogger.Client.Services;
public class APIService
{
    private readonly HttpClient _httpClient;
    public APIService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Shift> GetShiftAsync(int shiftId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/shifts/{shiftId}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var shift = JsonSerializer.Deserialize<Shift>(json);

            if (shift == null)
            {
                throw new InvalidOperationException("Failed to deserialize the Shift object. Please, try again.");
            }

            return shift;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error: {ex.Message}. Application will now close.");
            Environment.Exit(-1);
            return null;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error: {ex.Message}. Application will now close.");
            Environment.Exit(-1);
            return null;
        }
    }

    public async Task<List<Shift>> GetAllShiftsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/shifts");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var shifts = JsonSerializer.Deserialize<List<Shift>>(json);
            if (shifts == null)
            {
                throw new InvalidOperationException("Failed to deserialize the Shift object. Please, try again.");
            }
            return shifts;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error: {ex.Message}. Application will now close.");
            Environment.Exit(-1);
            return null;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error: {ex.Message}. Application will now close.");
            Environment.Exit(-1);
            return null;
        }

    }
}


