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
                new MenuRoute("Začít směnu", async () => await StartShift()),
                new MenuRoute("Ukončit směnu", async () => await EndShift()),
                new MenuRoute("Zobrazit směny", async () => await ViewShifts()),
                new MenuRoute("Konec", () =>
                {
                    Environment.Exit(0);
                    return Task.CompletedTask;
                })
            };

            var selectedRoute = AnsiConsole.Prompt(
                new SelectionPrompt<MenuRoute>()
                .Title("[yellow on bold]Vítejte v systému správy směn![/]\n-vyberte možnost-")
                .PageSize(10)
                .MoreChoicesText("[grey](Pro zobrazení dalších možností použijte šipky nahoru a dolů)[/]")
                .AddChoices(menuRoutes)
                );

            await selectedRoute.Action();
            Console.WriteLine("\nStiskněte libovolnou klávesu pro návrat do menu...");
            Console.ReadKey();
        }
    }

    private async Task StartShift()
    {
        AnsiConsole.MarkupLine("[yellow on bold]Zahájení nové směny[/]");
        var employeeDto = _userInput.GetEmployeeName();
        var response = await _apiService.StartShiftAsync(employeeDto);

        if (response.IsSuccessStatusCode)
        {
            AnsiConsole.MarkupLine("[green]Směna byla úspěšně zahájena![/]");
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            AnsiConsole.MarkupLine($"[red on bold]Nepodařilo se zahájit směnu. Chyba: {error}[/]");
        }
    }

    private async Task ViewShifts()
    {
        AnsiConsole.MarkupLine("[yellow on bold]Seznam směn[/]");
        var shifts = await _apiService.GetAllShiftsAsync();
        if (shifts == null || !shifts.Any())
        {
            AnsiConsole.MarkupLine("[yellow]Nebyly nalezeny žádné směny.[/]");
            return;
        }

        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Zaměstnanec");
        table.AddColumn("Začátek");
        table.AddColumn("Konec");
        table.AddColumn("Trvání");
        
        foreach (var shift in shifts)
        {
            table.AddRow(
                shift.Id.ToString(),
                shift.Employee,
                shift.Start.ToString("dd.MM.yyyy HH:mm"),
                shift.End?.ToString("dd.MM.yyyy HH:mm") ?? "Probíhá",
                shift.Duration?.ToString(@"hh\:mm\:ss") ?? "N/A"
            );
        }

        AnsiConsole.Write(table);
    }

    private async Task EndShift()
    {
        AnsiConsole.MarkupLine("[yellow on bold]Ukončení směny[/]");
        
        // Nejprve zobrazíme aktivní směny
        var shifts = await _apiService.GetAllShiftsAsync();
        var activeShifts = shifts.Where(s => s.End == null).ToList();
        
        if (!activeShifts.Any())
        {
            AnsiConsole.MarkupLine("[yellow]Nejsou žádné aktivní směny k ukončení.[/]");
            return;
        }
        
        // Zobrazíme tabulku aktivních směn
        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Zaměstnanec");
        table.AddColumn("Začátek");
        
        foreach (var shift in activeShifts)
        {
            table.AddRow(
                shift.Id.ToString(),
                shift.Employee,
                shift.Start.ToString("dd.MM.yyyy HH:mm")
            );
        }
        
        AnsiConsole.MarkupLine("[bold]Aktivní směny:[/]");
        AnsiConsole.Write(table);
        
        // Získáme ID směny k ukončení
        int shiftId = _userInput.GetShiftId();
        
        if (!activeShifts.Any(s => s.Id == shiftId))
        {
            AnsiConsole.MarkupLine("[red]Zadané ID neodpovídá žádné aktivní směně.[/]");
            return;
        }
        
        var response = await _apiService.EndShiftAsync(shiftId);

        if (response.IsSuccessStatusCode)
        {
            AnsiConsole.MarkupLine("[green]Směna byla úspěšně ukončena![/]");
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            AnsiConsole.MarkupLine($"[red]Nepodařilo se ukončit směnu. Chyba: {error}[/]");
        }
    }
}

