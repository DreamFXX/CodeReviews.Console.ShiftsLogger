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
            var response = await _httpClient.GetAsync($"api/shifts/{shiftId}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var shift = JsonSerializer.Deserialize<Shift>(json);

            if (shift == null)
            {
                throw new InvalidOperationException("Deserializace objektu Shift se nezdařila. Zkuste to prosím znovu.");
            }
            return shift;
        }
        catch (HttpRequestException ex)
        {
            AnsiConsole.Markup($"[red]Chyba: {ex.Message}. Aplikace bude nyní ukončena.[/]");
            Environment.Exit(0);
            return null;
        }
        catch (InvalidOperationException ex)
        {
            AnsiConsole.Markup($"[red]Chyba: {ex.Message}. Aplikace bude nyní ukončena.[/]");
            Environment.Exit(0);
            return null;
        }
    }

    public async Task<List<Shift>> GetAllShiftsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/shifts");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Shift>>(json) ?? new List<Shift>();
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"[red]Chyba při načítání směn: {ex.Message}[/]");
            return new List<Shift>();
        }
    }

    public async Task<HttpResponseMessage> StartShiftAsync(EmployeeDto employeeDto)
    {
        try
        {
            var json = JsonSerializer.Serialize(employeeDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync("api/shifts", content);
        }
        catch (HttpRequestException ex)
        {
            AnsiConsole.Markup($"[red]Chyba: {ex.Message}. Aplikace bude nyní ukončena.[/]");
            Environment.Exit(0);
            return null;
        }
    }

    public async Task<HttpResponseMessage> EndShiftAsync(int id)
    {
        try
        {
            return await _httpClient.PutAsync($"api/shifts/{id}", null);
        }
        catch (HttpRequestException ex)
        {
            AnsiConsole.Markup($"[red]Chyba: {ex.Message}. Aplikace bude nyní ukončena.[/]");
            Environment.Exit(0);
            return null;
        }
    }
}



