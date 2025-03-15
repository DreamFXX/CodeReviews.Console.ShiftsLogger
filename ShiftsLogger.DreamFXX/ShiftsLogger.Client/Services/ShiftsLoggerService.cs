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
        _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5031") };
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
                new MenuRoute("View Shift", async () => await ViewShifts()),
                new MenuRoute("Exit", () =>
                {
                    Environment.Exit(0);
                    return Task.CompletedTask;
                })
            };

            var selectedRoute = AnsiConsole.Prompt(
                new SelectionPrompt<MenuRoute>()
                .Title("[yellow on bold]Welcome to your Shift Management System![/]\n-select an option-")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .AddChoices(menuRoutes)
                );

            await selectedRoute.Action();
            Console.ReadKey();
        }
    }

    private async Task StartShift()
    {
        AnsiConsole.Markup("[yellow on bold]Enter the name of the employee starting the shift:[/]");
        var employeeName = _userInput.GetDetailsForStart();
        var response = await _apiService.StartShiftAsync(employeeName);

        if (response.IsSuccessStatusCode)
        {
            AnsiConsole.Markup("[green]Shift started successfully![/]");
        }
        else
        {
            AnsiConsole.Markup($"[red on bold]Failed to start the shift. Error: {response.StatusCode}[/]");
        }
    }

    private async Task ViewShifts()
    {
        var shifts = await _apiService.GetAllShiftsAsync();
        if (shifts == null || !shifts.Any())
        {
            AnsiConsole.Markup("[yellow]No shifts found..[/]");
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
            shift.Start.ToString(),
            shift.End?.ToString() ?? "Ongoing",
            shift.Duration?.ToString() ?? "N/A"
            );
        }

        AnsiConsole.Write(table);
    }

    private async Task EndShift()
    {
        AnsiConsole.Markup("[bold]Enter name of the employee to end shift:[/] ");
        var employeeName = _userInput.GetDetailsForStart();
        var response = await _apiService.EndShiftAsync(employeeName);

        if (response.IsSuccessStatusCode)
        {
            AnsiConsole.Markup("[green]Shift ended successfully![/]");
        }
        else
        {
            AnsiConsole.Markup($"[red]Failed to end the shift. Error: {response.StatusCode}[/] ");
        }
    }


}

