using System.Text;
using System.Text.Json;
using ShiftsLogger.Client.Models;
using Spectre.Console;

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
            Environment.Exit(0);
            return null;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error: {ex.Message}. Application will now close.");
            Environment.Exit(0);
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
            return JsonSerializer.Deserialize<List<Shift>>(json);
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"[red]Error retrieving shifts: {ex.Message}[/]");
            return new List<Shift>();
        }
    }

    public async Task<HttpResponseMessage> StartShiftAsync<EmployeeDto>(EmployeeDto employeeDto)
    {
        try
        {
            var json = JsonSerializer.Serialize(employeeDto);
            var content = new StringContent(json, Encoding.Unicode, "application/json");
            return await _httpClient.PostAsync("api/shifts", content);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error: {ex.Message}. Application will now close.");
            Environment.Exit(0);
            return null;
        }
    }

    public async Task<HttpResponseMessage> EndShiftAsync<EmployeeDto>(EmployeeDto employeeName)
    {
        try
        {
            var json = JsonSerializer.Serialize(employeeName);
            var content = new StringContent(json, Encoding.Default, "application/json");
            return await _httpClient.PutAsync("api/shifts", content);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error: {ex.Message}. Application will now close.");
            Environment.Exit(0);
            return null;
        }
    }
}



