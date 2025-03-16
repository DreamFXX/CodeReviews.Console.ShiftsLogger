using ShiftsLogger.Client.Models;
using ShiftsLogger.Client.UI;
using Spectre.Console;

namespace ShiftsLogger.Client.Services;

public class ShiftsLoggerService
{
    private readonly HttpClient _httpClient;
    private readonly APIService _apiService;
    private readonly ValidatorService _validatorService;
    private readonly UserInput _userInput;

    public ShiftsLoggerService()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5090") };
        _apiService = new APIService(_httpClient);
        _validatorService = new ValidatorService();
        _userInput = new UserInput(_validatorService);
    }

    public async Task Run()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var menuRoutes = new List<MenuRoute>
            {
                new MenuRoute("Start Shift", async () => await StartShift()),
                new MenuRoute("End Shift", async () => await EndShift()),
                new MenuRoute("View Shifts", async () => await ViewShifts()),
                new MenuRoute("Exit", () =>
                {
                    Environment.Exit(0);
                    return Task.CompletedTask;
                })
            };

            var selectedRoute = AnsiConsole.Prompt(
                new SelectionPrompt<MenuRoute>()
                .Title("[yellow on bold]Welcome to Shift Management System![/]\n-select an option-")
                .PageSize(10)
                .MoreChoicesText("[grey](Use up and down arrows to see more options)[/]")
                .AddChoices(menuRoutes)
                );

            await selectedRoute.Action();
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }
    }

    private async Task StartShift()
    {
        AnsiConsole.MarkupLine("[yellow on bold]Starting a new shift[/]");
        var employeeDto = _userInput.GetEmployeeName();
        var response = await _apiService.StartShiftAsync(employeeDto);

        if (response.IsSuccessStatusCode)
        {
            AnsiConsole.MarkupLine("[green]Shift started successfully![/]");
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            AnsiConsole.MarkupLine($"[red on bold]Failed to start shift. Error: {error}[/]");
        }
    }

    private async Task ViewShifts()
    {
        AnsiConsole.MarkupLine("[yellow on bold]Shift List[/]");
        var shifts = await _apiService.GetAllShiftsAsync();
        if (shifts == null || !shifts.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No shifts found.[/]");
            return;
        }

        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Employee");
        table.AddColumn("Start");
        table.AddColumn("End");
        table.AddColumn("Duration");
        
        foreach (var shift in shifts)
        {
            table.AddRow(
                shift.Id.ToString(),
                shift.Employee,
                shift.Start.ToString("MM/dd/yyyy HH:mm"),
                shift.End?.ToString("MM/dd/yyyy HH:mm") ?? "Ongoing",
                shift.Duration?.ToString(@"hh\:mm\:ss") ?? "N/A"
            );
        }

        AnsiConsole.Write(table);
    }

    private async Task EndShift()
    {
        AnsiConsole.MarkupLine("[yellow on bold]Ending a shift[/]");
        
        // First display active shifts
        var shifts = await _apiService.GetAllShiftsAsync();
        var activeShifts = shifts.Where(s => s.End == null).ToList();
        
        if (!activeShifts.Any())
        {
            AnsiConsole.MarkupLine("[yellow]There are no active shifts to end.[/]");
            return;
        }
        
        // Display table of active shifts
        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Employee");
        table.AddColumn("Start");
        
        foreach (var shift in activeShifts)
        {
            table.AddRow(
                shift.Id.ToString(),
                shift.Employee,
                shift.Start.ToString("MM/dd/yyyy HH:mm")
            );
        }
        
        AnsiConsole.MarkupLine("[bold]Active shifts:[/]");
        AnsiConsole.Write(table);
        
        // Get shift ID to end
        int shiftId = _userInput.GetShiftId();
        
        if (!activeShifts.Any(s => s.Id == shiftId))
        {
            AnsiConsole.MarkupLine("[red]The provided ID does not match any active shift.[/]");
            return;
        }
        
        var response = await _apiService.EndShiftAsync(shiftId);

        if (response.IsSuccessStatusCode)
        {
            AnsiConsole.MarkupLine("[green]Shift ended successfully![/]");
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            AnsiConsole.MarkupLine($"[red]Failed to end shift. Error: {error}[/]");
        }
    }
}

